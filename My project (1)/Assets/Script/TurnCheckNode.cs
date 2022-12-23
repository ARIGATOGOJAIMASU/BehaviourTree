using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCheckNode : CompositeNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //내턴 인 경우
        //return children[0].Update();

        return State.Running;
    }
}
