using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public enum PlayerType { Player, Enemy}
public enum TargetType { BaseAttack, Skill };

[System.Serializable]
public struct RunTimeStat
{
    public RunTimeStat(int base_HP, int base_MP, int base_STR, int base_INT, int base_AGI, int base_VLT, int base_LUK)
    {
        MaxHP = base_HP;
        CurHP = base_HP;
        MaxMP = base_MP;
        CurMP = 0;
        STR = base_STR;
        INT= base_INT;
        AGI = base_AGI;
        VLT = base_VLT;
        LUK = base_LUK;
    }

    public int MaxHP;
    public int CurHP;
    public int MaxMP;
    public int CurMP;
    public int STR;
    public int INT;
    public int AGI;
    public int VLT;
    public int LUK;
}

public class MyBuff
{
    public BuffType buffType;
    public Stat buffStat;
    public int duration;
    public int value;
    public Sprite icon;

    public MyBuff(BuffType _buffType, Stat _buffStat, int _duration, int _value, Sprite _icon)
    {
        buffType = _buffType;
        buffStat = _buffStat;
        duration = _duration;
        value = _value;
        icon = _icon;
    }

    public bool DurationCheck()
    {
        return --duration == 0;
    }
}

public enum Area { Front, Back }

[DefaultExecutionOrder(-1)]
public class Information : MonoBehaviour
{
    public HeroseStatData heroseDate;
    public List<SkillData> skillDatas;

    public PlayerType playrType;
    public RunTimeStat runTimeStat;

    public List<MyBuff> buffs = new ();

    //���� ��ȣ
    public int ID = 0;

    //�ڽ�����ġ
    public Vector3 startPos;
    public Quaternion startRotation;

    //�ڱ� �ڸ��� �ε�����ȣ
    public int indexNum = 0;

    public bool IsHurt = false;
    public bool IsDead = false;
    public bool OnUpdate = true;
    public bool UseSkill = false;

    //���� �������� SkillIndex
    public int curSkillIndex = 0;

    //�ڽ��� ��ġ
    public Area charcterArea;

    //Buff
    public GameObject buffUI_Base;
    [SerializeField] BuffManager buffUiManager;

    //Delegate
    public delegate void UseEffect(EffectName effectName, Vector3 start_Point, Vector3 forward);
    public UseEffect useEffect;

    //Dead Deleagete
    public delegate void PlayerDeadEvent(int ID);
    public PlayerDeadEvent playerDead;

    public delegate void UiActive(bool on);
    public UiActive uiActive;

    //Camera Delegate
    public delegate void CameraShaking();
    public CameraShaking cameraShaking;

    //Event
    public UnityEvent DeadEvent = new();
    private Animator playerAnimator;

    private void Start()
    {
        //��ü ����
        startPos = transform.position;

        //������ �ִ� ������ ���� ������ ����
        SettingRunTimeStat();
        playerAnimator = GetComponent<Animator>();

        DeadEvent.AddListener(() => playerDead(ID));
        DeadEvent.AddListener(() => uiActive(false));
    }

    //����ƽ������ �̸� ���
    public void SettingRunTimeStat()
    {
        runTimeStat = new RunTimeStat(heroseDate.HP, heroseDate.MP, heroseDate.STR, heroseDate.INT, heroseDate.AGI, heroseDate.VLT, heroseDate.LUK);

        /////////////////////////////////////////////////////////
        ///���� ���
        /////////////////////////////////////////////////////////     

        //////////////////////////////////////////////////////////
        ///������ ȿ�� ���
        //////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////
        ///Acitive Skill Ȯ��
        //////////////////////////////////////////////////////////
    }

    public int GetStatValue(Stat stat)
    {
        int finalValue = 0;

        switch (stat)
        {
            case Stat.STR:
                finalValue = runTimeStat.STR;
                break;
            case Stat.INT:
                finalValue = runTimeStat.INT;
                break;
            case Stat.AGI:
                finalValue = runTimeStat.AGI;
                break;
            case Stat.VLT:
                finalValue = runTimeStat.VLT;
                break;
            case Stat.LUK:
                finalValue = runTimeStat.LUK;
                break;
        }

        finalValue += GetBuffValue(stat);

        return finalValue;
    }

    public int GetSkillValue()
    {
        int Value = GetStatValue(skillDatas[curSkillIndex].BonusStatType) +
            (int)((float)GetStatValue(skillDatas[curSkillIndex].BonusStatType) * (skillDatas[curSkillIndex].BonusStatValue / 100));

        Value += GetBuffValue(Stat.Damage);

        return Value;
    }

    public void AddBuff(MyBuff buff)
    {
        buffs.Add(buff);

        //UI�߰�
        BuffUI buffUI = Instantiate(buffUI_Base, buffUiManager.transform).GetComponent<BuffUI>();
        buffUI.Setting(buff);
        buffUiManager.AddBuffUI(buffUI);
    }

    //���� Duration���� �� Check
    public void BuffCheck()
    {
        for(int i = 0; i < buffs.Count; ++i)
        {
            if(buffs[i].buffType == BuffType.TurnDamage)
            {
                runTimeStat.CurHP -= buffs[i].value;

                if(runTimeStat.CurHP <= 0)
                {
                    BattleManager.Instance.DeadChracter(ID);
                    return;
                }
            }

            //Duration�� 0�Ͻ� true��ȯ
            if(buffs[i].DurationCheck())
            {
                buffs.RemoveAt(i);
                --i;
            }
        }

        //buffUiCheck
        buffUiManager.BuffsDurationCheck();
    }

    public int GetBuffValue(Stat stat)
    {
        int buffValue = 0;

        for (int i = 0; i < buffs.Count; ++i)
        {
            if (buffs[i].buffStat == stat)
            {
                switch (buffs[i].buffType)
                {
                    case BuffType.Buff:
                        buffValue += buffs[i].value;
                        break;
                    case BuffType.DeBuff:
                        buffValue -= buffs[i].value;
                        break;
                }
            }
        }

        return buffValue;
    }

    public void Hurt(int damage)
    {
        IsHurt = true;
        //���� �˻� �� ������ ���� ������ Ȯ��
        damage -= (GetStatValue(Stat.AGI) / 5) + (int)((float)damage * (GetBuffValue(Stat.Damage) / 100.0f));

        if (damage > 0) runTimeStat.CurHP -= damage;

        if (runTimeStat.CurHP <= 0)
        {
            //Event����
            DeadEvent.Invoke();
        }

        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
            playerAnimator.Play("Hurt");
        }
        else
        {
            playerAnimator.Play("Hurt",0,0);
        }

        EffectEmerge(EffectName.Attack);
    }

    public void EffectEmerge(EffectName effectName)
    {
        useEffect(effectName, transform.position, transform.forward);
    }

    //�ִϸ��̼� �̺�Ʈ�� ȣ����
    public void HurtRelese()
    {
        IsHurt = false;
    }
}
