using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : DecoratorNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //�ڽ��� ���°� �� �ڽ��� ���°� �ȴ�.
        Child.Update();
        return State.Running;
    }
}
