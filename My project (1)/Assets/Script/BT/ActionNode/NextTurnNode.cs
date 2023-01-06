using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurnNode : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        BattleManager.Instance.NextChracter();
        return State.Success;
    }
}
