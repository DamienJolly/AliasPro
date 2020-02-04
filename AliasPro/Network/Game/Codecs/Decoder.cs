using AliasPro.Communication.Messages.Protocols;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System.Collections.Generic;
using System.Text;

namespace AliasPro.Network.Game.Codecs
{
	public class Decoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            if (input.GetByte(0) == 60)
            {
                context.WriteAndFlushAsync(Unpooled.CopiedBuffer(Encoding.UTF8.GetBytes("<?xml version=\"1.0\"?>\r\n" +
                        "<!DOCTYPE cross-domain-policy SYSTEM \"http://www.macromedia.com/xml/dtds/cross-domain-policy.dtd\">\r\n" +
                        "<cross-domain-policy>\r\n" +
                        "<allow-access-from domain=\"*\" to-ports=\"*\" />\r\n" +
                        "</cross-domain-policy>\0")));
            }
            else
            {
                input.MarkReaderIndex();
                int length = input.ReadInt();

                if (length <= input.ReadableBytes)
                {
                    output.Add(new ClientMessage(input.ReadBytes(length)));
                }
            }
        }
    }
}
