using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    public override void Init()
    {
    }

    protected override void OnStart() { }

    protected override void OnStop() { }

    protected override State OnUpdate()
    {
        //�ڽ� ��ü�� �ϳ��� Success�� ���ð�� �˻� ����
        for (int i= 0; i < children.Count; ++i)
        {
            State result = children[i].Update();

            if (result != State.Failure)
            {
                return result;
            }
        }

        return State.Failure;
    }
}
