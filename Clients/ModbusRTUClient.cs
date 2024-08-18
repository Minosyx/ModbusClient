using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MBClient.Clients
{
    public class ModbusRTUClient : ModbusClient
    {
        private readonly SerialPort _serialPort;
        private const int BufferSize = 256;

        public ModbusRTUClient(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate)
            {
                Parity = Parity.Even
            };
            _serialPort.Open();
        }

        protected override ModbusMessage ReadMessage()
        {
            var buffer = new byte[BufferSize];

            int bytesRead = _serialPort.Read(buffer, 0, BufferSize);

            var trimmedBuffer = new byte[bytesRead];
            Array.Copy(buffer, trimmedBuffer, bytesRead);

            ModbusMessage answer = new ModbusMessage(0x10, bytesRead - 4);
            Array.Copy(buffer, 1, answer.Data, 0, bytesRead - 3);
            CheckCRC(trimmedBuffer);
            return answer;
        }

        protected override bool WriteRequest(ModbusMessage modbusRequest)
        {
            byte address = 0x01;

            byte[] request = new byte[modbusRequest.Size + 3];
            request[0] = address;
            Array.Copy(modbusRequest.Data, 0, request, 1, modbusRequest.Size);
            request.FillTwoBytes(modbusRequest.Size + 1, request[..^2].CalculateCRC(), false);

            try
            {
                _serialPort.Write(request, 0, request.Length);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static void CheckCRC(byte[] buffer)
        {
            bool isCorrect = buffer.ValidateCRC();
            if (!isCorrect)
            {
                throw new Exception("CRC error");
            }
        }
    }
}
