using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationStateType { Idle, BaseAttack, Skill, Hurt, Move }

public class AnimationNode : ActionNode
{
    public string animationStateName;
    public bool OnRepeat;

    public override void Init()
    {
    }

    protected override void OnStart()
    {
        StartAnimation(animationStateName);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        AnimatorStateInfo Aniamtioninfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        if(OnRepeat) return State.Success;

        if (Aniamtioninfo.IsName(animationStateName) && Aniamtioninfo.normalizedTime > 0.95f)
        {
            return State.Success;
        }

        return State.Running;
    }

    void StartAnimation(string animationStateName)
    {
        //���� ���ư��� �ִϸ��̼��� �� �ִϸ��̼��� �´ٸ� return;
        playerAnimator.Play(animationStateName);
    }
}
