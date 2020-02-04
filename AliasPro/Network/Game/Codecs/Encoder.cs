using AliasPro.Communication.Messages.Protocols;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System.Collections.Generic;

namespace AliasPro.Network.Game.Codecs
{
    public class Encoder : MessageToMessageEncoder<ServerMessage>
    {
        protected override void Encode(IChannelHandlerContext context, ServerMessage message, List<object> output)
        {
            if (!message.HasLength)
            {
                message.Buffer.SetInt(0, message.Buffer.WriterIndex - 4);
            }

            output.Add(message.Buffer);
        }
    }
}
