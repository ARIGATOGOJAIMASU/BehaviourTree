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
        //�ڽ��� ĳ���Ͱ� �׾����� ��Ҵ����� �Ǵ� �� �ڽ� ��ü�� ����
        if(!ChacterManager.Instance.IsDead)
        {
            return Child.Update();
        }

        return State.Failure;
    }
}
