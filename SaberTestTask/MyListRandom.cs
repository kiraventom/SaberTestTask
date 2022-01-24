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