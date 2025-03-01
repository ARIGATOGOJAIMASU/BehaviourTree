using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//텍스트로 행동을 표현함
public class DebugLogNode : ActionNode
{
    public string message;

    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnStart()
    {
        Debug.Log($"OnStart{message}");
    }

    protected override void OnStop()
    {
        Debug.Log($"OnStop{message}");
    }

    protected override State OnUpdate()
    {
        Debug.Log($"OnUpdate{message}");
        return State.Success;
    }
}
