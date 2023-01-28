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
        //타켓을 쫓아감
        if ((OwnerTransform.position - OwnerBattle.targets[0].transform.position).magnitude > 0.5f)
        {
            OwnerTransform.position = Vector3.MoveTowards(OwnerTransform.position, OwnerBattle.targets[0].transform.position, Time.deltaTime * 30f);
            return State.Running;
        }
        //타켓 앞까지 옴
        else
        {
            //다음턴을 넘김
            //Info.action[0](Info.ID , AttackTarget);
            //MP업
            Info.runTimeStat.CurMP += Random.Range(0 , 50);
        }

        playerAnimator.SetBool(AnimationStateType.Move.ToString(), false);
        Info.curAnimationState = AnimationStateType.BaseAttack;
        return State.Success;
    }
}
