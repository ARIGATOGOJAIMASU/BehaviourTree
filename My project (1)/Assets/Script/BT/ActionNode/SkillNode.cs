using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNode : ActionNode
{
    Information[] targetInfos;
    Vector3 SkillPos;
    int index = 0;
    //enum SkillState { Success, Running, Failure }

    protected override void OnStart()
    {
        targetInfos = null;
        index = 0;
        Info.curSkillIndex = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Info.runTimeStat.CurMP >= 50)
        {
            Info.runTimeStat.CurMP = 0;
            Info.UseSkill = true;
            SetSkillPosition(index);
        }

        if (Info.UseSkill)
        {
            //실행중일때
            if (Info.skillDatas[0].SkillPosition == SkillPosition.Front
                && (OwnerTransform.position - SkillPos).magnitude > 0.5f)
            {
                OwnerTransform.position = Vector3.MoveTowards(OwnerTransform.position, SkillPos, Time.deltaTime * 30f);
                return State.Running;
            }
            //지정 위치에 도달 시 스킬 실행
            else
            {
                //스킬 사용
                BattleManager.Instance.UseSkill(Info, targetInfos, Info.skillDatas[index]);

                //다음 인덱스 ++
                ++index;
                ++Info.curSkillIndex;

                //스킬을 다 사용하거나 다음게임으로 진행이 불가능 할 시 들어감
                if (index == Info.skillDatas.Count || !BattleManager.Instance.PossibleNextGame())
                {
                    Info.UseSkill = false;
                    return State.Success;
                }
                //스킬이 남은 상태
                else
                {
                    SetSkillPosition(index);
                    return State.Running;
                }
            }
        }

        return State.Failure;
    }

    void SetSkillPosition(int index)
    {
        targetInfos = BattleManager.Instance.GetSkillTarget(Info.num ,Info.skillDatas[index], Info.playrType);

        switch (Info.skillDatas[index].SkillPosition)
        {
            case SkillPosition.Front:
                if (targetInfos.Length == 1) SkillPos = targetInfos[0].startPos;
                else SkillPos = targetInfos[0].startPos - (targetInfos[0].startPos - targetInfos[targetInfos.Length - 1].startPos) / 2;
                break;
            case SkillPosition.StartPos:
                SkillPos = Info.transform.position;
                break;
        }
    }
}