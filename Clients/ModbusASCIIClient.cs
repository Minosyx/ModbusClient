using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBClient.Clients
{
    public class ModbusASCIIClient : ModbusClient
    {
        private readonly SerialPort _serialPort;
        private const int BufferSize = 513;

        public ModbusASCIIClient(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate)
            {
                Parity = Parity.Even
            };
            _serialPort.Open();
        }

        protected override ModbusMessage ReadMessage()
        {
            var buffer = new char[BufferSize];

            int charsRead = _serialPort.Read(buffer, 0, BufferSize);

            var trimmedBuffer = new char[charsRead];
            Array.Copy(buffer, trimmedBuffer, charsRead);
            trimmedBuffer = trimmedBuffer[1..^2];
            byte[] trimmedBytes = trimmedBuffer.ToByteArray();
            ModbusMessage answer = new ModbusMessage(0x10, trimmedBytes.Length - 2);
            Array.Copy(trimmedBytes, 1, answer.Data, 0, trimmedBytes.Length - 2);
            CheckLRC(trimmedBytes);
            return answer;
        }

        protected override bool WriteRequest(ModbusMessage modbusRequest)
        {
            char start = ':';
            string end = "\r\n";

            byte address = 0x02;

            byte[] request = new byte[modbusRequest.Size + 2];
            request[0] = address;
            Array.Copy(modbusRequest.Data, 0, request, 1, modbusRequest.Size);
            request[modbusRequest.Size + 1] = request[..^1].CalculateLRC();

            string requestString = start + request.ToHexString() + end;

            try
            {
                _serialPort.Write(requestString);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static void CheckLRC(byte[] buffer)
        {
            bool isCorrect = buffer.ValidateLRC();
            if (!isCorrect)
            {
                throw new Exception("LRC check failed");
            }
        }
    }
}
