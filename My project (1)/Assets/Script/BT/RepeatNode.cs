using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : DecoratorNode
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
        //자신의 상태가 곧 자식의 상태가 된다.
        child.Update();

        return State.Running;
    }
}
