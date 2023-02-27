using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Battle : MonoBehaviour
{
    [SerializeField] Information myInfo;
    public List<Information> targets;

    //���� Ƚ��
    public int AttackCombo = 0;

    //Battle Effect
    [SerializeField] GameObject AttackEffectSource;
    [SerializeField] GameObject SkillEffectSource;
    EffectController AttackEffect;
    EffectController SkillEffect;

    //Target Delegate
    public delegate Information[] GetTarget(PlayerType playerType);
    public GetTarget getTarget;

    //Camera Delegate
    public delegate void CameraShaking(float duration, float magnitude);
    public CameraShaking cameraShaking;

    //Camera Delegate
    public delegate void CameraBattleMode(bool on);
    public CameraBattleMode battleMode;

    //Camera Target Delegate
    public CameraTarget cameraTarget;

    //TurnIndigate
    public GameObject turnIndigate;

    Vector3 EffectStartPoint;

    private void Start()
    {
        AttackEffect = Instantiate(AttackEffectSource).GetComponent<EffectController>();
        SkillEffect = Instantiate(SkillEffectSource).GetComponent<EffectController>();
    }

    public void GetBaseAttackTarget()
    {
        targets.Clear();
        Information[] infors = null;

        //��� ������ List�� ��ȯ
        infors = getTarget(myInfo.playerType == PlayerType.Player ? PlayerType.Enemy : PlayerType.Player);

        //////////////////////////////////////////////
        ///�켱���� ���ϴ� �Լ� ���� ����(Delegate�� Ȱ���Ͽ� �����غ���)
        //////////////////////////////////////////////
        targets.Add(infors[Random.Range(0, infors.Length - 1)]);
        EffectStartPoint = targets[0].startPos;

        if(myInfo.heroseDate.AttackType == AttackType.LONG)
        {
            //Camera.main.GetComponent<CameraController>().CameraBattleMode(true, targets[0].transform);
            cameraTarget.SetPosition(Vector3.Lerp(targets[0].transform.position, transform.position, 0.5f));
        }
        else
        {
            cameraTarget.SetTarget(transform);
        }

        battleMode(true);
    }

     public void Attack()
     {
        if (AttackCombo != 0)
        {
            targets[0].Hurt(myInfo.GetStatValue(Stat.STR) / 2);
        }
        else
        {
            targets[0].Hurt(myInfo.GetStatValue(Stat.STR));
        }

         //Playlog.text = $"{attackInfo.heroseDate.HeroseName}�� {targetObj[0].heroseDate.HeroseName}���� {attackInfo.GetStatValue(Stat.STR)}�� �⺻ ������ ��.";

         //0���� ���� �� ��� ó��
         /*if (targetObj[0].runTimeStat.CurHP <= 0)
         {
             DeadChracter(targetObj[0].ID);
         }*/
     }

    public void GetSkillTarget()
    {
        targets.Clear();
        //skill ����ϴ� Player����
        SkillData skillData = myInfo.skillDatas[myInfo.curSkillIndex];

        //Ÿ�ϼ��� ������
        List<Information> oppositeCamps = new();

        //Ÿ�� ���� �� ������ �����ϴ� �Լ�
        oppositeCamps = CheckArea(skillData);

        //�켱������ ���� Ÿ�� ����.
        switch (skillData.TargetPriority)
        {
            case TARGETPRIORITY.None:
                targets = oppositeCamps;
                break;
            case TARGETPRIORITY.Random:
                targets.Add(oppositeCamps[Random.Range(0, oppositeCamps.Count - 1)]);
                break;
            case TARGETPRIORITY.Hight_HP:
                {
                    int hight_HP = oppositeCamps[0].runTimeStat.CurHP;
                    int index = 0;

                    for (int i = 1; i < oppositeCamps.Count; ++i)
                    {
                        if (oppositeCamps[i].runTimeStat.CurHP > hight_HP)
                        {
                            hight_HP = oppositeCamps[i].runTimeStat.CurHP;
                            index = i;
                        }
                    }

                    targets.Add(oppositeCamps[index]);
                }
                break;
            case TARGETPRIORITY.Low_HP:
                {
                    int low_HP = oppositeCamps[0].runTimeStat.CurHP;
                    int index = 0;

                    for (int i = 1; i < oppositeCamps.Count; ++i)
                    {
                        if (oppositeCamps[i].runTimeStat.CurHP < low_HP)
                        {
                            low_HP = oppositeCamps[i].runTimeStat.CurHP;
                            index = i;
                        }
                    }

                    targets.Add(oppositeCamps[index]);
                }
                break;
            default:
                break;
        }
        //GetSkillEffectPosition();
    }

    public List<Information> CheckArea(SkillData skillData)
    {
        List<Information> oppositeCamps = new();

        //Ÿ�� ������ ����
        if (skillData.TargetPlayerType == PlayerType.Enemy)
            oppositeCamps.AddRange(myInfo.playerType == PlayerType.Player ? getTarget(PlayerType.Enemy): getTarget(PlayerType.Player));
        else
        oppositeCamps.AddRange(myInfo.playerType == PlayerType.Player ? getTarget(PlayerType.Player) : getTarget(PlayerType.Enemy));

    //Ÿ�� ������ ����
    switch (skillData.TargetRange)
        {
            case TargetArea.Self:
                {
                    List<Information> tempInfos = new();
                    tempInfos.Add(myInfo);
                    oppositeCamps = tempInfos;
                }
                break;

            case TargetArea.FrontRow:
                {
                    List<Information> tempInfos = new();

                    foreach (Information info in oppositeCamps)
                    {
                        if (info.charcterArea == Area.Front) tempInfos.Add(info);
                    }

                    if (tempInfos.Count != 0) oppositeCamps = tempInfos;
                }
                break;

            case TargetArea.BackRow:
                {
                    List<Information> tempInfos = new();

                    foreach (Information info in oppositeCamps)
                    {
                        if (info.charcterArea == Area.Back) tempInfos.Add(info);
                    }

                    if (tempInfos.Count != 0) oppositeCamps = tempInfos;
                }
                break;
            default:
                break;
        }
        return oppositeCamps;
    }

    public void UseSkill()
    {
        //������������ ������ �Ұ��� �� �� return;
        if (!BattleManager.Instance.PossibleNextGame())
        {
            return;
        }

        for (int i = 0; i < myInfo.skillDatas.Count; ++i)
        {
            SkillData skillData = myInfo.skillDatas[i];

            switch (skillData.SkillType)
            {
                case SKILLTYPE.ATTACK:
                    SkillAttack(skillData);
                    break;
                case SKILLTYPE.HELL:
                    SKillHeal(skillData);
                    break;
                case SKILLTYPE.BUFF:
                    SkillBuff(skillData);
                    break;
            }
        }
    }

    //SkillAction DelegateŰ���� ���
    public void SkillAttack(SkillData skillData)
    {
        int Damage = myInfo.GetSkillValue();

        //��ų �����. ���������� ���� Check
        if (myInfo.GetBuffValue(Stat.Damage) != 0)
        {
            Damage += (int)((float)Damage * ((float)myInfo.GetBuffValue(Stat.Damage) / 100.0f));
        }

        //�ǰ� ���ϴ� ���. ������ ���� ���� Check
        foreach (Information target in targets)
        {
            if (!target.IsDead)
            {
                target.Hurt(Damage);
            }
        }

        //targetObjs => { Hurt(Damage, deadChracterFuntion); }

        //Playlog.text = $"{useSkillObj.heroseDate.HeroseName}�� {Damage}�������� ��ų ����";
    }

    //SkillAction DelegateŰ���� ���
    public void SKillHeal(SkillData skillData)
    {
        foreach (Information target in targets)
        {
            target.Heal(myInfo.GetSkillValue());
            /*if (target.runTimeStat.CurHP + myInfo.GetSkillValue() > target.runTimeStat.MaxHP) target.runTimeStat.CurHP = target.runTimeStat.MaxHP;
            else target.runTimeStat.CurHP += myInfo.GetSkillValue();*/
        }

        //Playlog.text = $"{useSkillObj.heroseDate.HeroseName}�� {useSkillObj.GetSkillValue()}�� ���� ���";
    }

    //SkillAction DelegateŰ���� ���
    public void SkillBuff(SkillData skillData)
    {
        BuffSkillData buffData = (BuffSkillData)skillData;

        foreach (Information target in targets)
        {
            int buffValue = 0;

            switch (skillData.BonusStatType)
            {
                case Stat.None:
                    buffValue = (int)skillData.BonusStatValue;
                    break;
                default:
                    buffValue = (int)((float)myInfo.GetStatValue(skillData.BonusStatType) * skillData.BonusStatValue / 100.0f);
                    break;
            }

            Buff skillbuff = buffData.GetBuff;

            MyBuff buff = new MyBuff(skillbuff.buffType, skillbuff.buffStat, skillbuff.duration, buffValue, skillbuff.buffIcon);

            //Playlog.text = $"{useSkillObj.heroseDate.HeroseName}�� ������ų�� ���.";

            target.AddBuff(buff);
        }
    }

    //�ִϸ��̼� �̺�Ʈ�Լ�
    public void Action()
    {
        if (myInfo.UseSkill)
        {
            UseSkill();
        }
        else
        {
            Attack();
        }
    }

    //�ִϸ��̼� �̺�Ʈ�Լ�
    public void PlaySound()
    {
        if (myInfo.UseSkill)
        {
            SoundManager.Instance().Play(myInfo.soundList.Skill);
        }
        else
        {
            SoundManager.Instance().Play(myInfo.soundList.Attack);
        }
    }

    //�ִϸ��̼� �̺�Ʈ�Լ�
    public void BattleEffectEmerge()
    {
        if (myInfo.UseSkill)
        {
            GetSkillEffectPosition();

            if (SkillEffect.isActiveAndEnabled == false)
            {
                SkillEffect.StartEffect(EffectStartPoint, transform.forward);
            }
            else
            {
                SkillEffect.ReStart();
            }

            if(myInfo.skillDatas[myInfo.curSkillIndex].SkillType == SKILLTYPE.ATTACK) cameraShaking(0.15f, 1.2f);
        }
        else
        {
            if (AttackEffect.isActiveAndEnabled == false)
            {
                AttackEffect.StartEffect(EffectStartPoint, transform.forward);
            }
            else
            {
                AttackEffect.ReStart();
            }

            cameraShaking(0.15f, 0.8f);
        }
    }

    void GetSkillEffectPosition()
    {
        SkillData curSkillData = myInfo.skillDatas[myInfo.curSkillIndex];

        if (curSkillData.SkillType == SKILLTYPE.ATTACK)
        {
            //����� �ϳ�
            if (curSkillData.TargetRange == TargetArea.Single)
            {
                EffectStartPoint = myInfo.heroseDate.AttackType == AttackType.CLOSE ? transform.position : targets[0].startPos;
                cameraTarget.SetTarget(transform);
            }
            else
            {
                //�츮���� ������ ����
                if (curSkillData.TargetRange == TargetArea.FrontRow)
                {
                    EffectStartPoint = myInfo.playerType == PlayerType.Player ? Define.EnemyFrontPosition : Define.TeamFrontPosition;
                }
                else if (curSkillData.TargetRange == TargetArea.BackRow)
                {
                    EffectStartPoint = myInfo.playerType == PlayerType.Player ? Define.EnemyBackPosition : Define.TeamBackPosition;
                }
                else
                {
                    EffectStartPoint = myInfo.playerType == PlayerType.Player ? Define.EnemyMidPosition : Define.TeamMidPosition;
                }

                cameraTarget.SetPosition(Vector3.Lerp(transform.position, EffectStartPoint, 0.8f));
            }
        }
        else
        {
            if (curSkillData.TargetRange == TargetArea.Single)
            {
                EffectStartPoint = targets[0].startPos;
            }
            else
            {
                EffectStartPoint = myInfo.playerType == PlayerType.Player ? Define.TeamMidPosition : Define.EnemyMidPosition;
            }
        }
    }
}
