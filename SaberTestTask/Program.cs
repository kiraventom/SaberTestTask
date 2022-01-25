using SaberTestTask;

const string serializedFilename = "serialized.txt";
const string serializedCopyFilename = "serialized-copy.txt";

File.Delete(serializedFilename);
File.Delete(serializedCopyFilename);

var myListRandom = new MyListRandom();
myListRandom.Add("head");
myListRandom.Add("item0");
myListRandom.Add("item1");
myListRandom.Add("item2");
myListRandom.Add("item3");
myListRandom.Add("item4");
myListRandom.Add("tail");
myListRandom.SetRandoms();

using (var stream = File.Create(serializedFilename))
{
    myListRandom.Serialize(stream);
}


myListRandom = new MyListRandom();
using (var stream = File.OpenRead(serializedFilename))
{
    myListRandom.Deserialize(stream);
}

using (var stream = File.Create(serializedCopyFilename))
{
    myListRandom.Serialize(stream);
}

var contents0 = File.ReadAllText(serializedFilename);
var contents1 = File.ReadAllText(serializedCopyFilename);
var areSerializedEqual = contents0.Equals(contents1);
Console.WriteLine($"Serialized correctly: {areSerializedEqual}");
Console.ReadKey(true);