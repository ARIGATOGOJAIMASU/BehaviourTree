using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerNode : CompositeNode
{
    //���������� �ڽĳ�尡 �ϳ��� Failure�� ��ȯ�ϰų� ��� ��尡 Success�� ��ȯ�Ҷ����� �˻縦 �ǽ���

    int current;

    public override void Init()
    {
    }

    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {      
    }

    protected override State OnUpdate()
    {
        var child = children[current];

        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                return State.Failure;
            case State.Success:
                //������ �����ϸ� ���� ��带 �˻��ϱ� ���� �ε��� ���� ++
                ++current;
                break;
        }

        //�������� Success�� ��ȯ �� ��� Success�� ��ȯ
        return current == children.Count ? State.Success : State.Running;
    }
}
