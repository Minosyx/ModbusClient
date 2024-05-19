using System.Drawing;
using System.Reflection.Metadata.Ecma335;

namespace MBClient
{
    public class ModbusMessage
    {
        public byte[] Data { get; }

        public byte Function
        {
            get => Data[0];
            set => Data[0] = value;
        }

        public int Size => Data.Length;


        public ModbusMessage(byte function, int datasize)
        {
            Data = new byte[datasize + 1];
            Function = function;
        }
        
    }
}
