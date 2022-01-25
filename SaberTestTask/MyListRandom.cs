using System.Text;

namespace SaberTestTask;

public class MyListRandom : ListRandom
{
    private static readonly Random Random = new();

    public override void Serialize(Stream s)
    {
        MySerializer.Serialize(s, this);
    }

    public override void Deserialize(Stream s)
    {
        MySerializer.Deserialize(s, this);
    }

    public void Add(string data)
    {
        var newNode = new ListNode() {Data = data};
        Add(newNode);
    }

    public void Add(ListNode newNode)
    {
        if (Head is null)
        {
            Head = newNode;
        }
        else if (Head.Equals(Tail))
        {
            newNode.Previous = Head;
            Head.Next = newNode;
        }
        else
        {
            newNode.Previous = Tail;
            Tail.Next = newNode;
        }

        Tail = newNode;
        ++Count;
    }

    public void SetRandoms()
    {
        var node = Head;
        while (node is not null)
        {
            if (node.Random is null)
                node.Random = GetRandom();

            node = node.Next;
        }
    }

    public override bool Equals(object obj)
    {
        return obj is ListRandom listToCompareTo && Equals(listToCompareTo);
    }

    public bool Equals(ListRandom listToCompareTo)
    {
        if (listToCompareTo.Count != this.Count)
            return false;

        Dictionary<ListNode, int> indexedNodes = new();
        Dictionary<ListNode, int> indexedNodesToCompareTo = new();

        {   
            var node = this.Head;
            var nodeToCompareTo = this.Head;
            var index = 0;
            while (node is not null && nodeToCompareTo is not null)
            {
                indexedNodes.Add(node, index);
                indexedNodesToCompareTo.Add(nodeToCompareTo, index);

                node = node.Next;
                nodeToCompareTo = nodeToCompareTo.Next;
                ++index;
            }
        }

        {
            var node = this.Head;
            var nodeToCompareTo = this.Head;
            while (node is not null && nodeToCompareTo is not null)
            {
                if (!node.Data.Equals(nodeToCompareTo.Data))
                    return false;

                var nodeRandomId = indexedNodes[node.Random];
                var nodeRandomIdToCompareTo = indexedNodesToCompareTo[nodeToCompareTo.Random];
                if (nodeRandomId != nodeRandomIdToCompareTo)
                    return false;

                node = node.Next;
                nodeToCompareTo = nodeToCompareTo.Next;
            }
        }

        return true;
    }

    private ListNode GetRandom()
    {
        if (Count == 0)
            return null;

        var randomIndex = Random.Next(Count);
        var randomNode = Head;
        for (int i = 0; i < randomIndex; i++)
            randomNode = randomNode.Next;

        return randomNode;
    }
}