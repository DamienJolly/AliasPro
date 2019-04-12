using AliasPro.API.Network.Protocol;
using DotNetty.Buffers;
using System.Text;

namespace AliasPro.Network.Protocol
{
    public class ClientPacket : IClientPacket
    {
        public short Header { get; }
        private readonly IByteBuffer _buffer;

        public ClientPacket(IByteBuffer buffer)
        {
            Header = buffer.ReadShort();
            _buffer = buffer;
        }

        public short ReadShort() =>
            _buffer.ReadShort();

        public byte ReadByte() =>
            _buffer.ReadByte();

        public byte[] ReadBytes(int length) =>
            _buffer.ReadBytes(length).Array;

        public bool ReadBool() =>
            _buffer.ReadByte() == 1;

        public int ReadInt() =>
            _buffer.ReadInt();

        public string ReadString()
        {
            short length = _buffer.ReadShort();
            IByteBuffer data = _buffer.ReadBytes(length);
            return Encoding.Default.GetString(data.Array);
        }

        public override string ToString() =>
            Encoding.Default.GetString(_buffer.Array);
    }
}
