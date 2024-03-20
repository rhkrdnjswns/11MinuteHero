using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    private List<Node> childNodeList = new List<Node>();
    
    public abstract bool Execute(); //��� �б�
    public void AddChild(Node n)
    {
        childNodeList.Add(n);
    }
}

public class BehaviorTree
{
    private Node rootNode;
}
