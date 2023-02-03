using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNode : ActionNode
{
    Vector3 SkillPos;

    public override void Init()
    {
    }

    //enum SkillState { Success, Running, Failure }

    protected override void OnStart()
    {
        if (Info.UseSkill)
        {
            Info.curSkillIndex = 0;
            SetSkillPosition();
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
                //타켓이 한명이면
                if (OwnerBattle.targets.Count == 1) OwnerTransform.LookAt(OwnerBattle.targets[0].transform.position); 
                else
                OwnerTransform.rotation = Info.startRotation;

                return State.Success;
            }
        }

        return State.Failure;
    }

    void SetSkillPosition()
    {
        SkillData curSkillData = Info.skillDatas[Info.curSkillIndex];

        if (curSkillData.SkillStartPosition == SkillStartPosition.None)
        {
            switch (curSkillData.SkillPosition)
            {
                case SkillPosition.Front:
                    GetFrontSkillPosition();
                    break;
                case SkillPosition.StartPos:
                    SkillPos = Info.transform.position;
                    break;
            }
        }
        else if(curSkillData.SkillStartPosition == SkillStartPosition.Mid)
        {
            SkillPos = Define.ActionFrontPoint;
        }
        else
        {
            SkillPos = Info.playerType == PlayerType.Player ? Define.EnemyMidPosition : Define.TeamMidPosition;
        }
    }

    //근접공격 유형
    void GetFrontSkillPosition()
    {
        SkillData skillData = Info.skillDatas[Info.curSkillIndex];

        switch (skillData.TargetRange)
        {
            case TargetArea.FrontRow:
                SkillPos = Define.ActionFrontPoint;
                break;
            case TargetArea.Single:
                SkillPos = OwnerBattle.targets[0].transform.position +
                   (OwnerTransform.position - OwnerBattle.targets[0].transform.position).normalized * 2f;
                break;
            default:
                SkillPos = Info.playerType == PlayerType.Player ? Define.EnemyMidPosition : Define.TeamMidPosition;
                break;
        }
    }
}