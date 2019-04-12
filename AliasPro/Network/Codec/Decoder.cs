using AliasPro.Network.Protocol;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System.Collections.Generic;
using System.Text;

namespace AliasPro.Network.Codec
{
    internal class Decoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            input.MarkReaderIndex();

            if (input.ReadableBytes < 6) return;

            byte delimeter = input.ReadByte();
            input.ResetReaderIndex();

            if (delimeter == 60)
            {
                context.WriteAndFlushAsync(Unpooled.CopiedBuffer(Encoding.Default.GetBytes(Policy)));
            }
            else
            {
                input.MarkReaderIndex();
                int length = input.ReadInt();
                if (input.ReadableBytes < length)
                {
                    input.ResetReaderIndex();
                    return;
                }
                IByteBuffer newBuf = input.ReadBytes(length);

                if (length < 0) return;

                output.Add(new ClientPacket(newBuf));
            }
        }

        private string Policy =>
            "<?xml version=\"1.0\"?>\r\n"
                + "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n"
                + "<cross-domain-policy>\r\n"
                + "<allow-access-from domain=\"*\" to-ports=\"*\" />\r\n"
                + "</cross-domain-policy>\0)";
    }
}
