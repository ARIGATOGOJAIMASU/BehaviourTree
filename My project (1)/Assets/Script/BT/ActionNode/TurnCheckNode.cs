using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCheckNode : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(GameManager.Instance.TurnChracter == Info.num)
        {
            //�ڽ��� ���� ��
            return State.Success;
        }

        return State.Failure;
    }
}
