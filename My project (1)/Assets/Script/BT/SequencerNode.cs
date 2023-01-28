using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerNode : CompositeNode
{
    //시퀀스노드는 자식노드가 하나라도 Failure을 반환하거나 모든 노드가 Success을 반환할때까지 검사를 실시함

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
                //조건을 만족하면 다음 노드를 검색하기 위해 인덱스 값을 ++
                ++current;
                break;
        }

        //끝노드까지 Success을 반환 할 경우 Success을 반환
        return current == children.Count ? State.Success : State.Running;
    }
}
