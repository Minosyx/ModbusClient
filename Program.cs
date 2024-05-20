using System.Security.Cryptography;
using MBClient;

ModbusClient client = new ModbusTCPClient("localhost", 502);

//float[,] brightness = Bitmaps.GetBrightness(@"E:\Studia\IS2S3\pl.png", 10, 10);

//for (int i = 0; i < brightness.GetLength(0); i++){
//    for (int j = 0; j < brightness.GetLength(1); j++)
//    {
//        Console.Write($"{brightness[i, j]}");
//    }
//    Console.WriteLine();
//}

//for (int i = 0; i < 10; i++)
//{
//    var answer =
//        client.QA(new ModbusWMRRequest(Enumerable.Range(0, 10).Select(x => (ushort)(brightness[i, x] < 0.6 ? 8888 : 1)).ToArray(), (ushort)(10 * i)));
//}

//var readBitmap = client.QA(new ModbusRHRRequest(0, 100));

//for (int i = 0; i < 10; i++)
//{
//    for (int j = 0; j < 10; j++)
//    {

//        Console.Write($"{readBitmap.Data.GetTwoBytes(2 * (10 * i + j + 1))} ");
//    }
//    Console.WriteLine();
//}

//var readInputRegistersAnswer = client.QA(new ModbusRIRRequest(0, 4));

//for (int i = 0; i < 4; i++)
//{
//    Console.WriteLine(readInputRegistersAnswer.Data.GetTwoBytes(2 * (i + 1)));
//}

//var readDiscreteInputs = client.QA(new ModbusRDIRequest(0, 16));

//var byteCount = readDiscreteInputs.Data[1];

//for (int i = 0; i < byteCount; i++)
//{
//    for (int j = 0; j < 8; j++)
//    {
//        Console.Write((readDiscreteInputs.Data[2 + i] & (1 << j)) != 0 ? 1 : 0);
//    }
//}


byte[] inputs = [1,0,0,1,1,1,1,0,1,1,0,1,0,1,1,0];
ushort[] words = new ushort[inputs.Length / 8];
// group above into 8 bit words and store them Big Endian
for (int i = 0; i < words.Length; i++)
{
    for (int j = 0; j < 8; j++)
    {
        words[i] |= (ushort)((inputs[i * 8 + j] << (7 - j)) & (1 << (7 - j)));
    }
}

var writeCoilsAnswer = client.QA(new ModbusWMCRequest(0, 16, words));