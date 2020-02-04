using DotNetty.Buffers;
using System.Text;

namespace AliasPro.Communication.Messages.Protocols
{
    public class ServerMessage
    {
        public IByteBuffer Buffer { get; }

        public ServerMessage(short header)
        {
            Buffer = Unpooled.Buffer();
            WriteInt(-1);
            WriteShort(header);
        }

        public bool HasLength =>
            Buffer.GetInt(0) > -1;

        public void WriteInt(int value) =>
            Buffer.WriteInt(value);

        public void WriteShort(short value) =>
            Buffer.WriteShort(value);

        public void WriteByte(byte value) =>
            Buffer.WriteByte(value);

        public void WriteBoolean(bool value) =>
            Buffer.WriteBoolean(value);

        public void WriteString(string value)
        {
            Buffer.WriteShort(value.Length);
            Buffer.WriteBytes(Encoding.UTF8.GetBytes(value));
        }

        public void WriteDouble(double value) =>
            WriteString(value.ToString());
    }
}
