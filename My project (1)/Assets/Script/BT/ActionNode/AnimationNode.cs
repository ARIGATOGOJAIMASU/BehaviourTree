using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationStateType { Idle, BaseAttack, Skill, Hurt, Move}

public class AnimationNode : ActionNode
{
    public override void Init()
    {
    }

    protected override void OnStart()
    {
        playerAnimator.SetBool(Info.curAnimationState.ToString(), true);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        AnimatorStateInfo Aniamtioninfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        if(Aniamtioninfo.IsName(Info.curAnimationState.ToString()) && Aniamtioninfo.normalizedTime > 0.95)
        {
            playerAnimator.SetBool(Info.curAnimationState.ToString(), false); ;
            return State.Success;
        }

        return State.Running;
    }
}
