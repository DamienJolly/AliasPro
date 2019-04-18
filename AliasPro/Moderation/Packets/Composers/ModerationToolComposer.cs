using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationToolComposer : IPacketComposer
    {
        public ModerationToolComposer()
        {
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ModerationToolMessageComposer);
            // todo: tickets
            message.WriteInt(0); // count
            {
                // int:    ticketId
                // int:    ticketState
                // int:    ticketType
                // int:    ticketCategory
                // int:    ticketTime?
                // int:    ticketPriority?
                // int:    dunno?
                // int:    senderId
                // string: senderUsername
                // int:    reportedId
                // string: reportedUsername
                // int:    modId
                // string: modUsername
                // string: ticketCaption
                // int:    roomId
                // int:    dunno?
            }

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
