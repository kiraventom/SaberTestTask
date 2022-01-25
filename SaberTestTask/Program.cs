using SaberTestTask;

Tests.Serialize_FullList_ToFile();
Tests.Serialize_EmptyList_ToFile();
Tests.Serialize_FullList_ToMemory();
Tests.Serialize_EmptyList_ToMemory();
Tests.Deserialize_Valid_FromFile();
Tests.Deserialize_Invalid_FromFile();
Tests.Deserialize_Valid_FromMemory();
Tests.Deserialize_Invalid_FromMemory();

Console.WriteLine("Press any key...");
Console.ReadKey(true);