using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBClient
{
    public class ModbusASCIIClient : ModbusClient
    {
        private SerialPort serialPort;
        private const int BUFFER_SIZE = 513;

        public ModbusASCIIClient(string portName, int baudRate)
        {
            serialPort = new SerialPort(portName, baudRate)
            {
                Parity = Parity.Even
            };
            serialPort.Open();
        }

        protected override ModbusMessage ReadMessage()
        {
            var buffer = new char[BUFFER_SIZE];

            int charsRead = serialPort.Read(buffer, 0, BUFFER_SIZE);

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
                serialPort.Write(requestString);
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
