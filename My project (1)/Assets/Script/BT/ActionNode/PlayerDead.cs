using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : ActionNode
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
        Info.OnUpdate = false;

        return State.Success;
    }
}
