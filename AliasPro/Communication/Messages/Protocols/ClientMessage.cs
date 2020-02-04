using DotNetty.Buffers;
using System.Text;

namespace AliasPro.Communication.Messages.Protocols
{
    public class ClientMessage
    {
        private readonly IByteBuffer buf;

        public short Id { get; }

        public ClientMessage(IByteBuffer buf)
        {
            this.buf = buf;
            Id = ReadShort();
        }

        public short ReadShort() => 
            buf.ReadShort();

        public int ReadInt() => 
            buf.ReadInt();

        public int BytesAvailable() =>
            buf.ReadableBytes;

        public bool ReadBoolean() => 
            buf.ReadBoolean();

        public string ReadString() => 
            buf.ReadString(ReadShort(), Encoding.UTF8);
    }
}
