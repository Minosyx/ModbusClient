using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBClient
{
    public class ModbusRTUClient : ModbusClient
    {
        private SerialPort serialPort;

        public ModbusRTUClient(string portName, int baudRate)
        {
            serialPort = new SerialPort(portName, baudRate)
            {
                Parity = Parity.Even
            };
            serialPort.Open();
        }

        protected override ModbusMessage ReadMessage()
        {
            var buffer = new byte[256];
            serialPort.Read(buffer, 0, 3);
            int length = buffer[2] + 1;
            serialPort.Read(buffer, 3, length - 3);
            ModbusMessage answer = new ModbusMessage(0x10, length - 1);
            Array.Copy(buffer, 1, answer.Data, 0, length - 1);
            return answer;
        }

        protected override bool WriteRequest(ModbusMessage modbusRequest)
        {
            byte address = 0x0B;

            byte[] request = new byte[modbusRequest.Size + 3];
            request[0] = address;
            Array.Copy(modbusRequest.Data, 0, request, 1, modbusRequest.Size);
            request.FillTwoBytes(modbusRequest.Size + 1, request.CalculateCRC());
            
            try
            {
                serialPort.Write(request, 0, request.Length);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
