using System.Text;

namespace SaberTestTask;

public static class MySerializer
{
    private const string IdPropertyName = "Id";
    public static void Serialize(Stream stream, MyListRandom list)
    {
        Dictionary<ListNode, int> indexedNodes = new();

        {
            var index = 0;
            var node = list.Head;
            while (node is not null)
            {
                indexedNodes.Add(node, index++);
                node = node.Next;
            }
        }

        var writer = new StreamWriter(stream);
        writer.Write('[');
        
        {
            var node = list.Head;
            while (node is not null)
            {
                writer.Write('{');
                SerializeNode(writer, indexedNodes, node);
                writer.Write('}');
                writer.Write(',');
                node = node.Next;
            }
        }

        writer.Write(']');
        
        writer.Flush();
        // do not close the StreamWriter so stream won't close
    }

    private static void SerializeNode(TextWriter writer, IDictionary<ListNode, int> indexedNodes, ListNode node)
    {
        var index = indexedNodes[node];
        var randomIndex = indexedNodes[node.Random];
        SerializeProperty(writer, IdPropertyName, index.ToString());
        SerializeProperty(writer, nameof(node.Data), node.Data);
        SerializeProperty(writer, nameof(node.Random), randomIndex);
    }

    private static void SerializeProperty(TextWriter writer, string propertyName, object value)
    {
        writer.Write('"');
        writer.Write(propertyName);
        writer.Write('"');
        writer.Write(':');
        writer.Write('"');
        writer.Write(value);
        writer.Write('"');
        writer.Write(',');
    }
    public static void Deserialize(Stream stream, MyListRandom list)
    {
        var reader = new StreamReader(stream);
        Dictionary<int, ListNode> indexedNodes = new();
        Dictionary<ListNode, int> randomNodes = new();

        ReadUntil(reader, '[');
        while ((char) reader.Read() != ']')
        {
            var node = DeserializeNode(reader, indexedNodes, randomNodes);
            list.Add(node);
        }

        // fill randoms
        {
            var node = list.Head;
            while (node is not null)
            {
                var randomNodeExists = randomNodes.TryGetValue(node, out var randomId);
                if (randomNodeExists)
                {
                    var randomNode = indexedNodes[randomId];
                    node.Random = randomNode;
                }

                node = node.Next;
            }
        }
        
        // do not close the StreamReader so stream won't close
    }

    private static ListNode DeserializeNode(
        StreamReader reader, 
        IDictionary<int, ListNode> indexedNodes,
        IDictionary<ListNode, int> randomNodes)
    {
        ListNode node = new();

        while ((char) reader.Read() != '}')
        {
            var (name, value) = DeserializeProperty(reader);
            switch (name)
            {
                case nameof(node.Data):
                    node.Data = value;
                    break;
                
                case IdPropertyName:
                    var id = int.Parse(value);
                    indexedNodes.Add(id, node);
                    break;
                
                case nameof(node.Random):
                    var isRandom = int.TryParse(value, out var randomId);
                    if (isRandom)
                       randomNodes.Add(node, randomId); 
                    break;
                
                // can be expanded for new properties deserialization
            }
        }

        ReadUntil(reader, ',');
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
            var ch = (char) reader.Read();
            if (ch == toExclusive)
                break;

            builder.Append(ch);
        }

        return builder.ToString();
    }
}