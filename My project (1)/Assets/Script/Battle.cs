using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    [SerializeField] Information myInfo;
    public List<Information> targets;

    public delegate Information[] GetTarget(PlayerType playerType);
    public GetTarget getTarget;

    public void GetBaseAttackTarget()
    {
        Information[] infors = null;

        //��� ������ List�� ��ȯ
        infors = getTarget(myInfo.playrType == PlayerType.Player ? PlayerType.Enemy : PlayerType.Player);

        //////////////////////////////////////////////
        ///�켱���� ���ϴ� �Լ� ���� ����(Delegate�� Ȱ���Ͽ� �����غ���)
        //////////////////////////////////////////////
        targets.Add(infors[Random.Range(0, infors.Length - 1)]);
    }

     public void Attack()
     {
         targets[0].Hurt(myInfo.GetStatValue(Stat.STR));

         //Playlog.text = $"{attackInfo.heroseDate.HeroseName}�� {targetObj[0].heroseDate.HeroseName}���� {attackInfo.GetStatValue(Stat.STR)}�� �⺻ ������ ��.";

         //0���� ���� �� ��� ó��
         /*if (targetObj[0].runTimeStat.CurHP <= 0)
         {
             DeadChracter(targetObj[0].ID);
         }*/
     }

    public void GetSkillTarget()
    {
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
    }

    public List<Information> CheckArea(SkillData skillData)
    {
        List<Information> oppositeCamps = new();

        //Ÿ�� ������ ����
        if (skillData.TargetPlayerType == PlayerType.Enemy)
            oppositeCamps.AddRange(myInfo.playrType == PlayerType.Player ? getTarget(PlayerType.Enemy): getTarget(PlayerType.Player));
        else
        oppositeCamps.AddRange(myInfo.playrType == PlayerType.Player ? getTarget(PlayerType.Player) : getTarget(PlayerType.Enemy));

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
            target.Hurt(Damage);
        }

        //targetObjs => { Hurt(Damage, deadChracterFuntion); }

        //Playlog.text = $"{useSkillObj.heroseDate.HeroseName}�� {Damage}�������� ��ų ����";
    }

    //SkillAction DelegateŰ���� ���
    public void SKillHeal(SkillData skillData)
    {
        foreach (Information target in targets)
        {
            if (target.runTimeStat.CurHP + myInfo.GetSkillValue() > target.runTimeStat.MaxHP) target.runTimeStat.CurHP = target.runTimeStat.MaxHP;
            else target.runTimeStat.CurHP += myInfo.GetSkillValue();
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

    public void Action()
    {
        if (myInfo.UseSkill)
        {
            UseSkill();
        }
        else
            Attack();

        targets.Clear();
    }
}
