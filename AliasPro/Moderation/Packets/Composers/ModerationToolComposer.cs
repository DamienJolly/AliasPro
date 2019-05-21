using AliasPro.API.Moderation.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationToolComposer : IPacketComposer
    {
		private readonly ICollection<IModerationPreset> _userPresets;
		private readonly ICollection<IModerationPreset> _categoryPresets;
		private readonly ICollection<IModerationPreset> _roomPresets;
		private readonly ICollection<IModerationTicket> _tickets;

        public ModerationToolComposer(
			ICollection<IModerationPreset> userPresets,
			ICollection<IModerationPreset> categoryPresets,
			ICollection<IModerationPreset> roomPresets,
			ICollection<IModerationTicket> tickets)
        {
			_userPresets = userPresets;
			_categoryPresets = categoryPresets;
			_roomPresets = roomPresets;
			_tickets = tickets;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ModerationToolMessageComposer);
            message.WriteInt(_tickets.Count);
            foreach (IModerationTicket ticket in _tickets)
                ticket.Compose(message);

			message.WriteInt(_userPresets.Count);
			foreach (IModerationPreset preset in _userPresets)
				message.WriteString(preset.Data);

			message.WriteInt(_categoryPresets.Count);
			foreach (IModerationPreset preset in _categoryPresets)
				message.WriteString(preset.Data);

            // todo: permissions
            message.WriteBoolean(true); // view tickets
            message.WriteBoolean(true); // player chatlogs
            message.WriteBoolean(true); // send cautions
            message.WriteBoolean(true); // kick players
            message.WriteBoolean(true); // ban players
            message.WriteBoolean(true); // view room info?
            message.WriteBoolean(true); // room chatlogs?

			message.WriteInt(_roomPresets.Count);
			foreach (IModerationPreset preset in _roomPresets)
				message.WriteString(preset.Data);

			return message;
        }
	}
}
