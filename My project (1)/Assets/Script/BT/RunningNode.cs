using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningNode : DecoratorNode
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
        State state = child.Update();

        if(state == State.Running)
        {
            state = State.Success;
        }

        return state;
    }
}
