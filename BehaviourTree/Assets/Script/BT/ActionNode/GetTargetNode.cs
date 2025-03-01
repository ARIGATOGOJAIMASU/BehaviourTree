using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTargetNode : ActionNode
{
    public override void Init()
    {
    }

    protected override void OnStart()
    {
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
            //Info.curAnimationState = AnimationStateType.Skill;
            OwnerBattle.GetSkillTarget();
        }
        else
        {
            OwnerBattle.GetBaseAttackTarget();
        }
        
        return State.Success;
    }
}
