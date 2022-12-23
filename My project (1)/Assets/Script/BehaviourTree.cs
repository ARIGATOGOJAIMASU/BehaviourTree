using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public Node rootNode;
    public Node.State treeState = Node.State.Running;

    public Node.State Update()
    {
        //현재상태가 
        if (rootNode.state == Node.State.Running)
        {
            rootNode.Update();
        }

        return treeState;
    }
}
