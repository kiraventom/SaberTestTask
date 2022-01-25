using System.Diagnostics;

namespace SaberTestTask;

public static class Tests
{
    public static void Serialize_FullList_ToFile()
    {
        // Arrange
        var myListRandom = new MyListRandom();
        var head = new ListNode {Data = "head"};
        var node0 = new ListNode {Data = "node0"};
        var node1 = new ListNode {Data = "node1"};
        var tail = new ListNode {Data = "tail"};
        head.Random = node0;
        node0.Random = head;
        node1.Random = tail;
        tail.Random = head;
        myListRandom.Add(head);
        myListRandom.Add(node0);
        myListRandom.Add(node1);
        myListRandom.Add(tail);
        
        const string expected = "[{\"Id\":\"0\",\"Data\":\"head\",\"Random\":\"1\",}," +
                                "{\"Id\":\"1\",\"Data\":\"node0\",\"Random\":\"0\",}," +
                                "{\"Id\":\"2\",\"Data\":\"node1\",\"Random\":\"3\",}," +
                                "{\"Id\":\"3\",\"Data\":\"tail\",\"Random\":\"0\",},]";
        
        // Act
        var tempFilePath = Path.GetTempFileName();
        using (var stream = File.Create(tempFilePath))
        {
            myListRandom.Serialize(stream);
        }
        
        // Assert
        var actual = File.ReadAllText(tempFilePath);
        try
        {
            Debug.Assert(actual.Equals(expected));
            Console.WriteLine($"{nameof(Serialize_FullList_ToFile)} passed");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{nameof(Serialize_FullList_ToFile)} did not pass");
            Console.WriteLine($"Details: {e.Message}");
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }
    
    public static void Serialize_EmptyList_ToFile()
    {
        // Arrange
        var myListRandom = new MyListRandom();
        const string expected = "[]";

        // Act
        var tempFilePath = Path.GetTempFileName();
        using (var stream = File.Create(tempFilePath))
        {
            myListRandom.Serialize(stream);
        }
        
        // Assert
        var actual = File.ReadAllText(tempFilePath);
        try
        {
            Debug.Assert(actual.Equals(expected));
            Console.WriteLine($"{nameof(Serialize_EmptyList_ToFile)} passed");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{nameof(Serialize_EmptyList_ToFile)} did not pass");
            Console.WriteLine($"Details: {e.Message}");
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }
    
    public static void Serialize_FullList_ToMemory()
    {   
        // Arrange
        var myListRandom = new MyListRandom();
        var head = new ListNode {Data = "head"};
        var node0 = new ListNode {Data = "node0"};
        var node1 = new ListNode {Data = "node1"};
        var tail = new ListNode {Data = "tail"};
        head.Random = node0;
        node0.Random = head;
        node1.Random = tail;
        tail.Random = head;
        myListRandom.Add(head);
        myListRandom.Add(node0);
        myListRandom.Add(node1);
        myListRandom.Add(tail);
        
        const string expected = "[{\"Id\":\"0\",\"Data\":\"head\",\"Random\":\"1\",}," +
                                "{\"Id\":\"1\",\"Data\":\"node0\",\"Random\":\"0\",}," +
                                "{\"Id\":\"2\",\"Data\":\"node1\",\"Random\":\"3\",}," +
                                "{\"Id\":\"3\",\"Data\":\"tail\",\"Random\":\"0\",},]";
        
        // Act
        using var stream = new MemoryStream();
        myListRandom.Serialize(stream);
        
        // Assert
        stream.Position = 0;
        var reader = new StreamReader(stream);
        var actual = reader.ReadToEnd();
        try
        {
            Debug.Assert(actual.Equals(expected));
            Console.WriteLine($"{nameof(Serialize_FullList_ToMemory)} passed");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{nameof(Serialize_FullList_ToMemory)} did not pass");
            Console.WriteLine($"Details: {e.Message}");
        }
    }
    
    public static void Serialize_EmptyList_ToMemory()
    {
        // Arrange
        var myListRandom = new MyListRandom();
        const string expected = "[]";
        
        // Act
        using var stream = new MemoryStream();
        myListRandom.Serialize(stream);
        
        // Assert

        stream.Position = 0;
        var reader = new StreamReader(stream);
        var actual = reader.ReadToEnd();
        try
        {
            Debug.Assert(actual.Equals(expected));
            Console.WriteLine($"{nameof(Serialize_EmptyList_ToMemory)} passed");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{nameof(Serialize_EmptyList_ToMemory)} did not pass");
            Console.WriteLine($"Details: {e.Message}");
        }
    }
    
    public static void Deserialize_Valid_FromFile()
    {
        // Arrange
        const string serialized = "[{\"Id\":\"0\",\"Data\":\"head\",\"Random\":\"1\",}," +
                                  "{\"Id\":\"1\",\"Data\":\"node0\",\"Random\":\"0\",}," +
                                  "{\"Id\":\"2\",\"Data\":\"node1\",\"Random\":\"3\",}," +
                                  "{\"Id\":\"3\",\"Data\":\"tail\",\"Random\":\"0\",},]";

        var tempFilePath = Path.GetTempFileName();
        File.WriteAllText(tempFilePath, serialized);
        
        var expected = new MyListRandom();
        var head = new ListNode {Data = "head"};
        var node0 = new ListNode {Data = "node0"};
        var node1 = new ListNode {Data = "node1"};
        var tail = new ListNode {Data = "tail"};
        head.Random = node0;
        node0.Random = head;
        node1.Random = tail;
        tail.Random = head;
        expected.Add(head);
        expected.Add(node0);
        expected.Add(node1);
        expected.Add(tail);

        // Act
        var actual = new MyListRandom();
        using (var stream = File.OpenRead(tempFilePath))
        {
            actual.Deserialize(stream);
        }

        // Assert
        try
        {
            Debug.Assert(expected.Equals(actual));
            Console.WriteLine($"{nameof(Deserialize_Valid_FromFile)} passed");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{nameof(Deserialize_Valid_FromFile)} did not pass");
            Console.WriteLine($"Details: {e.Message}");
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }
    
    public static void Deserialize_Invalid_FromFile()
    {
        // Arrange
        const string serialized = "[{\"Id\":\"0\",\"Data\":\"head\",\"Random\":\"1\",}," +
                                  "{\"Id\":\"1\",\"Data\":\"node0\",\"Random\":\"2\",}," +
                                  "{\"Id\":\"2\",\"Data\":\"tail\",\"Random\":\"0\",},]";

        var tempFilePath = Path.GetTempFileName();
        File.WriteAllText(tempFilePath, serialized);
        
        var expected = new MyListRandom();
        var head = new ListNode {Data = "head"};
        var node0 = new ListNode {Data = "node0"};
        var node1 = new ListNode {Data = "node1"};
        var tail = new ListNode {Data = "tail"};
        head.Random = node0;
        node0.Random = head;
        node1.Random = tail;
        tail.Random = head;
        expected.Add(head);
        expected.Add(node0);
        expected.Add(node1);
        expected.Add(tail);

        // Act
        var actual = new MyListRandom();
        using (var stream = File.OpenRead(tempFilePath))
        {
            actual.Deserialize(stream);
        }

        // Assert
        try
        {
            Debug.Assert(!expected.Equals(actual));
            Console.WriteLine($"{nameof(Deserialize_Invalid_FromFile)} passed");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{nameof(Deserialize_Invalid_FromFile)} did not pass");
            Console.WriteLine($"Details: {e.Message}");
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }
    
    public static void Deserialize_Valid_FromMemory()
    {
        // Arrange
        const string serialized = "[{\"Id\":\"0\",\"Data\":\"head\",\"Random\":\"1\",}," +
                                  "{\"Id\":\"1\",\"Data\":\"node0\",\"Random\":\"0\",}," +
                                  "{\"Id\":\"2\",\"Data\":\"node1\",\"Random\":\"3\",}," +
                                  "{\"Id\":\"3\",\"Data\":\"tail\",\"Random\":\"0\",},]";

        var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        writer.Write(serialized);
        writer.Flush();
        stream.Position = 0;
        
        var expected = new MyListRandom();
        var head = new ListNode {Data = "head"};
        var node0 = new ListNode {Data = "node0"};
        var node1 = new ListNode {Data = "node1"};
        var tail = new ListNode {Data = "tail"};
        head.Random = node0;
        node0.Random = head;
        node1.Random = tail;
        tail.Random = head;
        expected.Add(head);
        expected.Add(node0);
        expected.Add(node1);
        expected.Add(tail);

        // Act
        var actual = new MyListRandom();
        actual.Deserialize(stream);

        // Assert
        try
        {
            Debug.Assert(expected.Equals(actual));
            Console.WriteLine($"{nameof(Deserialize_Valid_FromMemory)} passed");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{nameof(Deserialize_Valid_FromMemory)} did not pass");
            Console.WriteLine($"Details: {e.Message}");
        }
    }
    
    public static void Deserialize_Invalid_FromMemory()
    {
        // Arrange
        const string serialized = "[{\"Id\":\"0\",\"Data\":\"head\",\"Random\":\"1\",}," +
                                  "{\"Id\":\"1\",\"Data\":\"node0\",\"Random\":\"2\",}," +
                                  "{\"Id\":\"2\",\"Data\":\"tail\",\"Random\":\"0\",},]";

        var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        writer.Write(serialized);
        writer.Flush();
        stream.Position = 0;
        
        var expected = new MyListRandom();
        var head = new ListNode {Data = "head"};
        var node0 = new ListNode {Data = "node0"};
        var node1 = new ListNode {Data = "node1"};
        var tail = new ListNode {Data = "tail"};
        head.Random = node0;
        node0.Random = head;
        node1.Random = tail;
        tail.Random = head;
        expected.Add(head);
        expected.Add(node0);
        expected.Add(node1);
        expected.Add(tail);

        // Act
        var actual = new MyListRandom();
        actual.Deserialize(stream);

        // Assert
        try
        {
            Debug.Assert(!expected.Equals(actual));
            Console.WriteLine($"{nameof(Deserialize_Invalid_FromMemory)} passed");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{nameof(Deserialize_Invalid_FromMemory)} did not pass");
            Console.WriteLine($"Details: {e.Message}");
        }
    }
}