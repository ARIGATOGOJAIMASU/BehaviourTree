using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : Node
{
    //�ڽ� ��带 ������ �ִ� ����
    //SequencerNode �� SelectoNode�� �ش�
    public List<Node> children = new List<Node>();
}
