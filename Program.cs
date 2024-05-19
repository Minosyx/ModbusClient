using MBClient;

ModbusClient client = new ModbusTCPClient("localhost", 502);

float[,] brightness = Bitmaps.GetBrightness(@"C:\Users\HUAWEI\Downloads\great-britain-flag.gif", 10, 10);

for (int i = 0; i < brightness.GetLength(0); i++){
    for (int j = 0; j < brightness.GetLength(1); j++)
    {
        Console.Write($"{brightness[i, j]}");
    }
    Console.WriteLine();
}

for (int i = 0; i < 10; i++)
{
    var answer =
        client.QA(new ModbusWMRRequest(Enumerable.Range(0, 10).Select(x => (ushort)(brightness[i, x] < 0.6 ? 8888 : 1)).ToArray(), (ushort)(10 * i)));
}