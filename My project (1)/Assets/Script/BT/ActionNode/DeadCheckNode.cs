using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCheckNode : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Debug.Log("데드 체크");
        if (!Info.IsDead)
        {
            return State.Success;
        }

        Info.OnUpdate = false;
        Debug.Log("죽음");
        OwnerTransform.Rotate(Vector3.forward * -90, Space.World);
        return State.Failure;
    }
}
