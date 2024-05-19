


namespace MBClient
{
    public abstract class ModbusClient
    {
        public ModbusMessage? QA(ModbusMessage modbusRequest)
        {
            return !WriteRequest(modbusRequest) ? null : ReadMessage();
        }

        protected abstract ModbusMessage ReadMessage();

        protected abstract bool WriteRequest(ModbusMessage modbusRequest);
    }
}
