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
            playerAnimator.SetBool(AnimationStateType.Move.ToString(), true);
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
                OwnerTransform.position = Vector3.MoveTowards(OwnerTransform.position, SkillPos, Time.deltaTime * 30f);
                return State.Running;
            }
            else
            {
                playerAnimator.SetBool(AnimationStateType.Move.ToString(), false);
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
                if (OwnerBattle.targets.Count == 1) SkillPos = OwnerBattle.targets[0].startPos;
                else SkillPos = 
                        OwnerBattle.targets[0].startPos - 
                        (OwnerBattle.targets[0].startPos - OwnerBattle.targets[OwnerBattle.targets.Count - 1].startPos) / 2;
                break;
            case SkillPosition.StartPos:
                SkillPos = Info.transform.position;
                break;
        }
    }
}