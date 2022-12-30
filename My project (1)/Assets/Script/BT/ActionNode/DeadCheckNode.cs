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
        if (!Info.IsDead)
        {
            Debug.Log("w");
            return State.Success;
        }

        Debug.Log("d");
        Info.OnUpdate = false;
        OwnerTransform.gameObject.SetActive(false);
        return State.Failure;
    }
}
