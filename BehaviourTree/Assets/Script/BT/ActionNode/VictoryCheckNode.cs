using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCheckNode : ActionNode
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
        if(Info.victory)
        {
            //Info.OnUpdate = false;
            return State.Success;
        }

        return State.Failure;
    }
}
