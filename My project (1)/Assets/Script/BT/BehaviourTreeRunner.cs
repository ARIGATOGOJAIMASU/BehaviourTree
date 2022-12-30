using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;
    Information info;

    private void Start()
    {
        info = GetComponent<Information>();

        Debug.Log(info);
        Debug.Log(transform);
        tree = tree.Clone(info, transform);
        /*Debug.Log(info);
        tree = ScriptableObject.CreateInstance<BehaviourTree>();
        var repeatNode = ScriptableObject.CreateInstance<RepeatNode>();
        //----------------------------------------------------------------------------------------------------------------------------------
        var selectorNode = ScriptableObject.CreateInstance<SequencerNode>();
        repeatNode.child = selectorNode;
        //----------------------------------------------------------------------------------------------------------------------------------
        var sequencerNode1 = ScriptableObject.CreateInstance<SequencerNode>();
        selectorNode.children.Add(sequencerNode1);

        var TrunCheckNode = ScriptableObject.CreateInstance<TurnCheckNode>();
        TrunCheckNode.Info = info;
        sequencerNode1.children.Add(TrunCheckNode);

        var Selecotr2 = ScriptableObject.CreateInstance<SelectorNode>();
        sequencerNode1.children.Add(Selecotr2);

        var AttackMove = ScriptableObject.CreateInstance<AttackMoveNode>();
        AttackMove.Info = info;
        AttackMove.OwnerTransform = transform;
        sequencerNode1.children.Add(AttackMove);

        //var Decorator = ScriptableObject.CreateInstance<DecoratorNode>();


        //--------------------------------------------------------------------------------------------------------------------------------------------------
        var sequencerNode2 = ScriptableObject.CreateInstance<SequencerNode>();
        selectorNode.children.Add(sequencerNode2);

        var DeadCheck = ScriptableObject.CreateInstance<DeadCheckNode>();
        DeadCheck.Info = info;
        DeadCheck.OwnerTransform = transform;
        sequencerNode2.children.Add(DeadCheck);

        var BackMoceNode = ScriptableObject.CreateInstance<BackMoveNode>();
        BackMoceNode.Info = info;
        BackMoceNode.OwnerTransform = transform;
        sequencerNode2.children.Add(BackMoceNode);

        //-------------------------------------------------------------------------------------------------------------------------------------------------
        */

        //tree.rootNode = repeatNode;
    }

    private void Update()
    {
/*        if (info.OnUpdate)
        {*/
            tree.Update();
      //  }
    }
}
 