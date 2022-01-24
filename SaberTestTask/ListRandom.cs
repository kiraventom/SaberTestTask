namespace SaberTestTask;

public class ListNode
{
    public ListNode Previous;
    public ListNode Next;
    public ListNode Random; // произвольный элемент внутри списка
    public string Data;
}

public class ListRandom
{
    public ListNode Head;
    public ListNode Tail;
    public int Count;

    public virtual void Serialize(Stream s)
    {
    }

    public virtual void Deserialize(Stream s)
    {
    }
}