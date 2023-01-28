using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMoveNode : ActionNode
{
    public override void Init()
    {
    }

    protected override void OnStart()
    {
        playerAnimator.SetBool(AnimationStateType.Move.ToString(), true);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        //Ÿ���� �Ѿư�
        if ((OwnerTransform.position - OwnerBattle.targets[0].transform.position).magnitude > 0.5f)
        {
            OwnerTransform.position = Vector3.MoveTowards(OwnerTransform.position, OwnerBattle.targets[0].transform.position, Time.deltaTime * 30f);
            return State.Running;
        }
        //Ÿ�� �ձ��� ��
        else
        {
            //�������� �ѱ�
            //Info.action[0](Info.ID , AttackTarget);
            //MP��
            Info.runTimeStat.CurMP += Random.Range(0 , 50);
        }

        playerAnimator.SetBool(AnimationStateType.Move.ToString(), false);
        Info.curAnimationState = AnimationStateType.BaseAttack;
        return State.Success;
    }
}
