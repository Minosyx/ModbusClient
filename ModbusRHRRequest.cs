using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBClient
{
    public class ModbusRHRRequest : ModbusMessage
    {
        public ModbusRHRRequest(ushort startAddress, ushort quantity) : base(0x03, 4)
        {
            Data.FillTwoBytes(1, startAddress);
            Data.FillTwoBytes(3, quantity);
        }
    }
}
