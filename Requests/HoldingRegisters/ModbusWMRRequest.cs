namespace MBClient.Requests.HoldingRegisters
{
    public class ModbusWMRRequest : ModbusMessage
    {
        public ModbusWMRRequest(ushort[] values, ushort startAddress) : base(0x10, 5 + 2 * values.Length)
        {
            Data.FillTwoBytes(1, startAddress);
            Data.FillTwoBytes(3, (ushort)values.Length);
            Data[5] = (byte)(2 * values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                Data.FillTwoBytes(6 + 2 * i, values[i]);
            }
        }
    }
}
