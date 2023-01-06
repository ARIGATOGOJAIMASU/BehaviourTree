using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMoveNode : ActionNode
{
    Information targetInfo;

    protected override void OnStart()
    {
        targetInfo = null;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(targetInfo == null)
        {
            targetInfo = BattleManager.Instance.GetBaseAttackTarget(Info.playrType);
        }

        //Ÿ���� �Ѿư�
        if ((OwnerTransform.position - targetInfo.transform.position).magnitude > 0.5f)
        {
            OwnerTransform.position = Vector3.MoveTowards(OwnerTransform.position, targetInfo.transform.position, Time.deltaTime * 30f);
            return State.Running;
        }
        //Ÿ�� �ձ��� ��
        else
        {
            //�������� �ѱ�
            BattleManager.Instance.Attack(Info ,targetInfo);
            //MP��
            Info.runTimeStat.CurMP += Random.Range(0 , 50);
        }

        return State.Success;
    }
}
