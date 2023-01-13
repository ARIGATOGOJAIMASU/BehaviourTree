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

        //Ÿ���� �Ѿư�
        if ((OwnerTransform.position - AttackTarget[0].transform.position).magnitude > 0.5f)
        {
            OwnerTransform.position = Vector3.MoveTowards(OwnerTransform.position, AttackTarget[0].transform.position, Time.deltaTime * 30f);
            return State.Running;
        }
        //Ÿ�� �ձ��� ��
        else
        {
            //�������� �ѱ�
            Info.actionDelegates[0](Info.num , AttackTarget);
            //MP��
            Info.runTimeStat.CurMP += Random.Range(0 , 50);
        }

        return State.Success;
    }
}
