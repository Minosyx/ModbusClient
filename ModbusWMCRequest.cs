using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBClient
{
    public class ModbusWMCRequest : ModbusMessage
    {
        public ModbusWMCRequest(ushort[] values, ushort startAddress, ushort quantity) : base(0x0F, 5 + 2 * values.Length)
        {
            Data.FillTwoBytes(1, startAddress);
            Data.FillTwoBytes(3, quantity);
            Data[5] = (byte) (2 * values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                Data.FillTwoBytes(6 + 2 * i, values[i]);
            }
        }
    }
}
