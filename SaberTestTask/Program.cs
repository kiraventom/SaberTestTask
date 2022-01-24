using SaberTestTask;

{
    var myListRandom = new MyListRandom();
    myListRandom.Add("head");
    myListRandom.Add("item0");
    myListRandom.Add("item1");
    myListRandom.Add("item2");
    myListRandom.Add("tail");
    myListRandom.SetRandoms();

    var stream = new MemoryStream();
    myListRandom.Serialize(stream);
    stream.Position = 0;
    using var reader = new StreamReader(stream);
    var str = reader.ReadToEnd();
    File.WriteAllText("D:\\text0.txt", str);
}

{
    var newListRandom = new MyListRandom();
    newListRandom.Deserialize(File.OpenRead("D:\\text0.txt"));

    var stream = new MemoryStream();
    newListRandom.Serialize(stream);
    stream.Position = 0;
    using var reader = new StreamReader(stream);
    var str = reader.ReadToEnd();

    File.WriteAllText("D:\\text1.txt", str);
}
Console.WriteLine();