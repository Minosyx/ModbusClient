using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBClient.Requests.InputRegisters
{
    public class ModbusRIRRequest : ModbusMessage
    {
        public ModbusRIRRequest(ushort startAddress, ushort quantity) : base(0x04, 4)
        {
            Data.FillTwoBytes(1, startAddress);
            Data.FillTwoBytes(3, quantity);
        }
    }
}
