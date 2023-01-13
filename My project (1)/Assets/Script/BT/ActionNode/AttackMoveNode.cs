using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMoveNode : ActionNode
{
    Information[] AttackTarget;

    protected override void OnStart()
    {
        AttackTarget = null;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(AttackTarget == null)
        {
            AttackTarget = Info.getTargetDelegates[0](Info.num);
        }

        //타켓을 쫓아감
        if ((OwnerTransform.position - AttackTarget[0].transform.position).magnitude > 0.5f)
        {
            OwnerTransform.position = Vector3.MoveTowards(OwnerTransform.position, AttackTarget[0].transform.position, Time.deltaTime * 30f);
            return State.Running;
        }
        //타켓 앞까지 옴
        else
        {
            //다음턴을 넘김
            Info.actionDelegates[0](Info.num , AttackTarget);
            //MP업
            Info.runTimeStat.CurMP += Random.Range(0 , 50);
        }

        return State.Success;
    }
}
