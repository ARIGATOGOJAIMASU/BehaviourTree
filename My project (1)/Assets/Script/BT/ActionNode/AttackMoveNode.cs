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

        //타켓을 쫓아감
        if ((OwnerTransform.position - targetInfo.transform.position).magnitude > 0.5f)
        {
            OwnerTransform.position = Vector3.MoveTowards(OwnerTransform.position, targetInfo.transform.position, Time.deltaTime * 30f);
            return State.Running;
        }
        //타켓 앞까지 옴
        else
        {
            //다음턴을 넘김
            BattleManager.Instance.Attack(Info ,targetInfo);
            //MP업
            Info.runTimeStat.CurMP += Random.Range(0 , 50);
        }

        return State.Success;
    }
}
