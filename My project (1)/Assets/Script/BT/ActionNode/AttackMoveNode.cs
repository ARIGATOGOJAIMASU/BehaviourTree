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

        //Ÿ���� �Ѿư�
        if ((OwnerTransform.position - targetInfo.transform.position).magnitude > 0.5f)
        {
            OwnerTransform.position = Vector3.MoveTowards(OwnerTransform.position, targetInfo.transform.position, Time.deltaTime * 20f);
            return State.Running;
        }
        //Ÿ�� �ձ��� ��
        else
        {
            //�������� �ѱ�
            GameManager.Instance.Attack(Info ,targetInfo);
        }

        return State.Success;
    }
}
