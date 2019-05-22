using AliasPro.API.Moderation.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Permissions;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationToolComposer : IPacketComposer
    {
		private readonly IPermissionsController _permissionsController;
		private readonly IPlayer _player;
		private readonly ICollection<IModerationPreset> _userPresets;
		private readonly ICollection<IModerationPreset> _categoryPresets;
		private readonly ICollection<IModerationPreset> _roomPresets;
		private readonly ICollection<IModerationTicket> _tickets;

        public ModerationToolComposer(
			IPermissionsController permissionsController,
			IPlayer player,
			ICollection<IModerationPreset> userPresets,
			ICollection<IModerationPreset> categoryPresets,
			ICollection<IModerationPreset> roomPresets,
			ICollection<IModerationTicket> tickets)
        {
			_permissionsController = permissionsController;
			_player = player;
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

			message.WriteBoolean(_permissionsController.HasPermission(_player, "acc_modtool_ticket_queue"));
			message.WriteBoolean(_permissionsController.HasPermission(_player, "acc_modtool_player_logs"));
			message.WriteBoolean(_permissionsController.HasPermission(_player, "acc_modtool_player_alert"));
			message.WriteBoolean(_permissionsController.HasPermission(_player, "acc_modtool_player_kick"));
			message.WriteBoolean(_permissionsController.HasPermission(_player, "acc_modtool_player_ban"));
			message.WriteBoolean(_permissionsController.HasPermission(_player, "acc_modtool_room_info")); //not sure?
			message.WriteBoolean(_permissionsController.HasPermission(_player, "acc_modtool_room_logs")); //not sure?

			message.WriteInt(_roomPresets.Count);
			foreach (IModerationPreset preset in _roomPresets)
				message.WriteString(preset.Data);

			return message;
        }
	}
}
