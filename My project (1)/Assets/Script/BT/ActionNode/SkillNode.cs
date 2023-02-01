using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNode : ActionNode
{
    Vector3 SkillPos;
    int index = 0;

    public override void Init()
    {
    }

    //enum SkillState { Success, Running, Failure }

    protected override void OnStart()
    {
        if (Info.UseSkill)
        {
            index = 0;
            Info.curSkillIndex = 0;
            SetSkillPosition(index);
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Info.UseSkill)
        {
            //스킬사용 위치까지 이동
            if (Info.skillDatas[0].SkillPosition == SkillPosition.Front
                && (OwnerTransform.position - SkillPos).magnitude > 0.5f)
            {
                OwnerTransform.position = Vector3.MoveTowards(OwnerTransform.position, SkillPos, Time.deltaTime * 10f);
                return State.Running;
            }
            else
            {
                OwnerTransform.rotation = Info.startRotation;
                return State.Success;
            }
        }

        return State.Failure;
    }

    void SetSkillPosition(int index)
    {
        switch (Info.skillDatas[index].SkillPosition)
        {
            case SkillPosition.Front:
                GetFrontSkillPosition();
                break;
            case SkillPosition.StartPos:
                SkillPos = Info.transform.position;
                break;
        }
    }

    void GetFrontSkillPosition()
    {
        SkillData skillData = Info.skillDatas[Info.curSkillIndex];

        switch (skillData.TargetRange)
        {
            case TargetArea.All:
                SkillPos = Info.playrType == PlayerType.Player ? Define.EnemyMidPosition : Define.TeamMidPosition;
                break;
            default:
                SkillPos = Define.ActionFrontPoint;
                break;
        }
    }
}