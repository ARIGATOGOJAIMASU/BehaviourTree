using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    protected override void OnStart() { }

    protected override void OnStop() { }

    protected override State OnUpdate()
    {
        //자식 객체중 하나라도 Success가 나올경우 검사 종료
        for (int i= 0; i < children.Count; ++i)
        {
            if (children[i].Update() == State.Success && children[i].Update() == State.Running)
            {
                return State.Success;
            }
        }

        return State.Failure;
    }
}
