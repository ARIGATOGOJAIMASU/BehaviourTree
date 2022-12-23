using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDeadNode : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {      
    }

    protected override State OnUpdate()
    {
        //자신의 캐릭터가 죽었는지 살았는지를 판단 후 자식 객체에 접근
        if(!ChacterManager.Instance.IsDead)
        {
            return Child.Update();
        }

        return State.Failure;
    }
}
