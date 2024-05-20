using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBClient
{
    public class ModbusRDIRequest : ModbusMessage
    {
        public ModbusRDIRequest(ushort startAddress, ushort quantity) : base(0x02, 4)
        {
            Data.FillTwoBytes(1, startAddress);
            Data.FillTwoBytes(3, quantity);
        }
    }
}
