using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System.Collections.Generic;

namespace AliasPro.Network.Codec
{
    using Protocol;

    internal class Encoder : MessageToMessageEncoder<ServerPacket>
    {
        protected override void Encode(IChannelHandlerContext context, ServerPacket message, List<object> output)
        {
            if (!message.HasLength)
            {
                message.ByteBuffer.SetInt(0, message.ByteBuffer.WriterIndex - 4);
            }

            output.Add(message.ByteBuffer);
        }
    }
}
