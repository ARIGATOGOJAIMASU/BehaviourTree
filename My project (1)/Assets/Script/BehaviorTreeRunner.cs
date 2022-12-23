using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
    BehaviourTree tree;

    private void Start()
    {
        tree = ScriptableObject.CreateInstance<BehaviourTree>();

        var log1 = ScriptableObject.CreateInstance<DebugLogNode>();
        log1.message = "Hello1";

        var log2 = ScriptableObject.CreateInstance<DebugLogNode>();
        log2.message = "Hello2";

        var log3 = ScriptableObject.CreateInstance<DebugLogNode>();
        log3.message = "Hello3";

        var Sequence = ScriptableObject.CreateInstance<SequencerNode>();
        Sequence.children.Add(log1);
        Sequence.children.Add(log2);
        Sequence.children.Add(log3);

        var loop = ScriptableObject.CreateInstance<RepeatNode>();
        loop.Child = Sequence;

        tree.rootNode = loop;
    }

    private void Update()
    {
        tree.Update();
    }
}
