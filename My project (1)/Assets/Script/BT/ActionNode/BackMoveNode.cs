using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMoveNode : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if((OwnerTransform.position - Info.startPos).magnitude > 0.5f)
        {
            OwnerTransform.position = Vector3.Lerp(OwnerTransform.position, Info.startPos, Time.deltaTime * 4);
            return State.Running;
        }

        return State.Success;
    }
}
