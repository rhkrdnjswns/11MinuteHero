
public class BehaviorTree
{
    private readonly Node root;
    public BehaviorTree(Node node)
    {
        root = node;
    }
    public void Execute()
    {
        root.Execute();
    }
}
public abstract class Node
{
    public abstract bool Execute(); //노드 읽기
}
public class Selector : Node
{
    private readonly Node[] childNodeArray;
    public override bool Execute()
    {
        foreach (var item in childNodeArray)
        {
            if (item.Execute()) return true;
        }
        return false;
    }
    public Selector(params Node[] nodes)
    {
        childNodeArray = nodes;
    }
}
public class Sequence : Node
{
    private readonly Node[] childNodeArray;
    public override bool Execute()
    {
        foreach (var item in childNodeArray)
        {
            if (!item.Execute()) return false;
        }
        return true;
    }
    public Sequence(params Node[] nodes)
    {
        childNodeArray = nodes;
    }
}

public class Condition : Node
{
    private readonly System.Func<bool> condition;
    private readonly Node decoratedNode;
    public Condition(System.Func<bool> condition, Node node)
    {
        this.condition = condition;
        decoratedNode = node;
    }
    public override bool Execute()
    {
        if (condition())
        {
            return decoratedNode.Execute();
        }
        return false;
    }
}
public class Action : Node
{
    private readonly System.Action action;
    public Action(System.Action action)
    {
        this.action = action;
    }
    public override bool Execute()
    {
        action();
        return true;
    }
}
public class Inverter : Node //자식 노드의 반환값을 반전시켜 반환
{
    private Node decoratedNode;
    public Inverter(Node node)
    {
        decoratedNode = node;
    }
    public override bool Execute()
    {
        return !decoratedNode.Execute();
    }
}

