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
        if((OwnerTransform.position - Info.StartPos).magnitude > 0.5f)
        {
            OwnerTransform.position = Vector3.Lerp(OwnerTransform.position, Info.StartPos, Time.deltaTime * 4);
            return State.Running;
        }

        Debug.Log("µé¾î °¬´Ù³×");
        return State.Success;
    }
}
