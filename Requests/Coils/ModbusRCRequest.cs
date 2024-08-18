using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBClient.Requests.Coils
{
    public class ModbusRCRequest : ModbusMessage
    {
        public ModbusRCRequest(ushort startAddress, ushort quantity) : base(0x01, 4)
        {
            Data.FillTwoBytes(1, startAddress);
            Data.FillTwoBytes(3, quantity);
        }
    }
}
