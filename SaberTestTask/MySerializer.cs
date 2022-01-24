using System.Text;

namespace SaberTestTask;

public static class MySerializer
{
    public static void Serialize(Stream stream, MyListRandom list)
    {
        Dictionary<int, ListNode> serializedNodes = new();
        var builder = new StringBuilder();
        
        builder.Append('[');
        var node = list.Head;
        while (node is not null)
        {
            builder.Append('{');
            SerializeNode(builder, serializedNodes, node);
            builder.Append('}').Append(',');
            node = node.Next;
        }
        
        builder.Append(']');

        var bytes = Encoding.UTF8.GetBytes(builder.ToString());
        stream.Write(bytes);
    }

    private static void SerializeNode(StringBuilder builder, IDictionary<int, ListNode> serializedNodes, ListNode node)
    {
        var hashcode = node.GetHashCode();
        SerializeProperty(builder, "Hash", hashcode.ToString());
        SerializeProperty(builder, nameof(node.Data), node.Data);
        serializedNodes.Add(hashcode, node);
    }

    private static void SerializeProperty(StringBuilder builder, string propertyName, string value)
    {
        builder
            .Append('"').Append(propertyName).Append('"')
            .Append(':')
            .Append('"').Append(value).Append('"')
            .Append(',');
    }

    public static void Deserialize(Stream stream, MyListRandom list)
    {
        using var reader = new StreamReader(stream);

        ReadUntil(reader, '[');
        while ((char) reader.Read() != ']')
        {
            var node = DeserializeNode(reader);
            list.Add(node);
        }
    }

    private static ListNode DeserializeNode(StreamReader reader)
    {
        ListNode node = new();
        
        while ((char) reader.Read() != '}')
        {
            var (name, value) = DeserializeProperty(reader);
            var fieldInfo = typeof(ListNode).GetField(name);
            fieldInfo?.SetValue(node, value);
        }

        reader.Read(); // comma between nodes
        return node;
    }

    private static (string name, string value) DeserializeProperty(StreamReader reader)
    {
        var name = ReadUntil(reader, '"');
        ReadUntil(reader, ':');
        ReadUntil(reader, '"');
        var value = ReadUntil(reader, '"');
        ReadUntil(reader, ',');
        return (name, value);
    }

    private static string ReadUntil(StreamReader reader, char toExclusive)
    {
        StringBuilder builder = new();
        while (!reader.EndOfStream)
        {
            var ch = (char)reader.Read();
            if (ch == toExclusive)
                break;

            builder.Append(ch);
        }

        return builder.ToString();
    }
}