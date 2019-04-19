﻿using DotNetty.Buffers;
using System.Text;

namespace AliasPro.Network.Protocol
{
    public class ServerPacket
    {
        public IByteBuffer ByteBuffer { get; }

        public ServerPacket(short header)
        {
            ByteBuffer = Unpooled.Buffer();
            ByteBuffer.WriteInt(-1);
            ByteBuffer.WriteShort(header);
        }

        public bool HasLength =>
            ByteBuffer.GetInt(0) > -1;

        public void WriteInt(int i) =>
            ByteBuffer.WriteInt(i);

        public void WriteInt(uint i) =>
            ByteBuffer.WriteInt((int)i);

        public void WriteShort(int s) =>
            ByteBuffer.WriteShort(s);

        public void WriteByte(int b) =>
            ByteBuffer.WriteByte((byte)b);

        public void WriteBoolean(bool b) =>
            ByteBuffer.WriteByte(b ? 1 : 0);

        public void WriteString(string s)
        {
            ByteBuffer.WriteShort(s.Length);
            ByteBuffer.WriteBytes(Encoding.UTF8.GetBytes(s));
        }

        public void WriteDouble(double d) =>
            ByteBuffer.WriteDouble(d);
    }
}
