using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMoveNode : ActionNode
{
    public override void Init()
    {
    }

    protected override void OnStart()
    {
        playerAnimator.SetBool(AnimationStateType.Move.ToString(), true);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if ((OwnerTransform.position - Info.startPos).magnitude > 0.5f)
        {
            OwnerTransform.position = Vector3.Lerp(OwnerTransform.position, Info.startPos, Time.deltaTime * 4);
            return State.Running;
        }

        playerAnimator.SetBool(AnimationStateType.Move.ToString(), false);
        return State.Success;
    }
}
