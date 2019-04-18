using AliasPro.API.Moderation.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationToolComposer : IPacketComposer
    {
        private readonly ICollection<IModerationTicket> _tickets;

        public ModerationToolComposer(ICollection<IModerationTicket> tickets)
        {
            _tickets = tickets;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ModerationToolMessageComposer);
            message.WriteInt(_tickets.Count);
            foreach (IModerationTicket ticket in _tickets)
                ticket.Compose(message);

            // todo: user presets
            message.WriteInt(0); // count
            {
                // string: presetCaption
            }

            // todo: category presets
            message.WriteInt(0); // count
            {
                // string: presetCaption
            }

            // todo: permissions
            message.WriteBoolean(true); // view tickets
            message.WriteBoolean(true); // player chatlogs
            message.WriteBoolean(true); // send cautions
            message.WriteBoolean(true); // kick players
            message.WriteBoolean(true); // ban players
            message.WriteBoolean(true); // view room info?
            message.WriteBoolean(true); // room chatlogs?

            // todo: room presets
            message.WriteInt(0); // count
            {
                // string: presetCaption
            }
            return message;
        }
    }
}
