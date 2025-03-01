using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCheckNode : ActionNode
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
        if (!Info.IsDead) return State.Failure;

        return State.Success;
    }
}
