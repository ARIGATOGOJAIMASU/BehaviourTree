using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { Player, Enemy}

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
    //레벨 정보
    //아이템 정보

    public PlayerType playrType;
    public RunTimeStat runTimeStat;

    public List<MyBuff> buffs = new ();

    //식별을 위한 고유번호
    public int num = 0;
    public bool IsHurt;
    public Vector3 startPos;
    public bool IsDead = false;
    public bool OnUpdate = true;
    public bool UseSkill = false;
    public Area charcterArea;
    public GameObject buffUI_Base;
    public int curSkillIndex = 0;
    [SerializeField] BuffManager buffUiManager;

    private void Start()
    {
        //교체 예정
        startPos = transform.position;

        //가지고 있는 정보를 토대로 스탯을 수정
        SettingRunTimeStat();
    }

    public void SettingRunTimeStat()
    {
        runTimeStat = new RunTimeStat(heroseDate.HP, heroseDate.MP, heroseDate.STR, heroseDate.INT, heroseDate.AGI, heroseDate.VLT, heroseDate.LUK);
    }

    public int GetSTR()
    {
        int finalSTR = runTimeStat.STR;

        finalSTR += GetBuffValue(Stat.STR);

        return runTimeStat.STR;
    }

    public int GetINT()
    {
        int finalINT = runTimeStat.INT;

        finalINT += GetBuffValue(Stat.INT);

        return runTimeStat.INT;
    }

    public int GetAGI()
    {
        int finalAGI = runTimeStat.AGI;

        finalAGI += GetBuffValue(Stat.AGI);
        return runTimeStat.AGI;
    }

    public int GetVLT()
    {
        int finalVLT = runTimeStat.VLT;

        finalVLT += GetBuffValue(Stat.VLT);

        return runTimeStat.VLT;
    }

    public int GetLUK()
    {
        int finalLUK = runTimeStat.LUK;

        finalLUK += GetBuffValue(Stat.LUK);

        return runTimeStat.LUK;
    }

    public int GetSkillValue()
    {
        int Value = 0;

        switch(skillDatas[curSkillIndex].BonusStatType)
        {
            case Stat.INT:
                Value = GetINT() + (int)((float)GetINT() * (skillDatas[curSkillIndex].BonusStatValue/ 100));
                break;
            case Stat.STR:
                Value = GetSTR() + (int)((float)GetSTR() * (skillDatas[curSkillIndex].BonusStatValue / 100));
                break;
        }

        Value += GetBuffValue(Stat.Damage);

        return Value;
    }

    public void AddBuff(MyBuff buff)
    {
        buffs.Add(buff);

        //UI추가
        BuffUI buffUI = Instantiate(buffUI_Base, buffUiManager.transform).GetComponent<BuffUI>();
        buffUI.Setting(buff);

        buffUiManager.AddBuffUI(buffUI);
    }

    //버프 Duration감소 및 Check
    public void BuffCheck()
    {
        for(int i = 0; i < buffs.Count; ++i)
        {
            if(buffs[i].buffType == BuffType.TurnDamage)
            {
                runTimeStat.CurHP -= buffs[i].value;

                if(runTimeStat.CurHP <= 0)
                {
                    BattleManager.Instance.DeadChracter(this);
                    return;
                }
            }

            //Duration이 0일시 true반환
            if(buffs[i].DurationCheck())
            {
                buffs.RemoveAt(i);
                --i;
            }
        }

        //UiCheck
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
        //방어력 검사 및 데미지 감소 버프들 확인
        damage -= (GetAGI()/ 5) + (int)((float)damage * (GetBuffValue(Stat.Damage) / 100.0f));

        if (GetBuffValue(Stat.Damage) != 0)
        {
            Debug.Log((int)((float)damage * (GetBuffValue(Stat.Damage) / 100.0f)));
        }

        if (damage > 0) runTimeStat.CurHP -= damage;

        if (runTimeStat.CurHP <= 0)
        {
            BattleManager.Instance.DeadChracter(this);
        }
    }

    public void OnMouseEnter()
    {
        OnInfomation();
    }

    public void OnMouseExit()
    {
        OffInfomation();
    }

    public void OnInfomation()
    {
        BattleManager.Instance.statExpantionUI.SetStatExplanation(this);
        BattleManager.Instance.statExpantionUI.gameObject.SetActive(true);
    }

    public void OffInfomation()
    {
        BattleManager.Instance.statExpantionUI.gameObject.SetActive(false);
    }
}
