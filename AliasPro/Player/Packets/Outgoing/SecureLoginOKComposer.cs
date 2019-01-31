using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Player.Packets.Outgoing
{
    public class SecureLoginOKComposer : ServerPacket
    {
        public SecureLoginOKComposer()
            : base(OutgoingHeaders.SecureLoginOKMessageComposer)
        {
        }
    }
}
