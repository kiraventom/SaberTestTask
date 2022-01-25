namespace SaberTestTask;

public class MyListRandom : ListRandom
{
    public override void Serialize(Stream s)
    {
        MySerializer.Serialize(s, this);
    }

    public override void Deserialize(Stream s)
    {
        MySerializer.Deserialize(s, this);
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

    public override bool Equals(object obj)
    {
        return obj is ListRandom listToCompareTo && Equals(listToCompareTo);
    }

    public bool Equals(ListRandom listToCompareTo)
    {
        if (Count != listToCompareTo.Count)
            return false;

        Dictionary<ListNode, int> indexedNodes = new();
        Dictionary<ListNode, int> indexedNodesToCompareTo = new();

        {   
            var node = Head;
            var nodeToCompareTo = listToCompareTo.Head;
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
            var node = Head;
            var nodeToCompareTo = listToCompareTo.Head;
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
}