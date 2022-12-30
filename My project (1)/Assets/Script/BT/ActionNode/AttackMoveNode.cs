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
            if (Info.playrType == PlayrType.Player)
            {
                targetInfo = GameManager.Instance.enemyInformations[Random.Range(0, GameManager.Instance.enemyInformations.Count - 1)];
            }
            else
            {
                targetInfo = GameManager.Instance.playerInformations[Random.Range(0, GameManager.Instance.playerInformations.Count - 1)];
            }
        }

        //타켓을 쫓아감
        if ((OwnerTransform.position - targetInfo.transform.position).magnitude > 0.5f)
        {
            OwnerTransform.position = Vector3.MoveTowards(OwnerTransform.position, targetInfo.transform.position, Time.deltaTime * 20f);
            return State.Running;
        }
        //타켓 앞까지 옴
        else
        {
            //다음턴을 넘김
            GameManager.Instance.Attack(Info ,targetInfo);
        }

        return State.Success;
    }
}
