using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCheckNode : ActionNode
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
        if(BattleManager.Instance.TurnChracter == Info.ID)
        {
            //�ڽ��� ���� ��
            return State.Success;
        }

        return State.Failure;
    }
}
