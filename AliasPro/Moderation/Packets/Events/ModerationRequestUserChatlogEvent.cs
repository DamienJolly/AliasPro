using AliasPro.API.Chat;
using AliasPro.API.Chat.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Permissions;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Network.Events.Headers;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRequestUserChatlogEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationRequestUserChatlogMessageEvent;

		private readonly IPlayerController _playerController;
		private readonly IChatController _chatController;
		private readonly IPermissionsController _permissionsController;

		public ModerationRequestUserChatlogEvent(
			IPlayerController playerController,
			IChatController chatController,
			IPermissionsController permissionsController)
		{
			_playerController = playerController;
			_chatController = chatController;
			_permissionsController = permissionsController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_player_logs"))
				return;

			int playerId = clientPacket.ReadInt();

            IPlayerData player = await _playerController.GetPlayerDataAsync((uint)playerId);
            if (player == null)
                return;

            IDictionary<IPlayerRoomVisited, ICollection<IChatLog>> chatlogs = 
                new Dictionary<IPlayerRoomVisited, ICollection<IChatLog>>();

            foreach (IPlayerRoomVisited visit in
                await _playerController.GetPlayerRoomVisitsAsync(player.Id))
            {
                if (!chatlogs.TryAdd(visit, new List<IChatLog>()))
                    continue;

                IList<IChatLog> logs = new List<IChatLog>();
                chatlogs[visit] = await _chatController.ReadRoomChatlogs(
                    (uint)visit.RoomId, 
                    visit.EntryTimestamp, 
                    visit.ExitTimestamp);
            }
            
            await session.SendPacketAsync(new ModeratorUserChatlogComposer(player, chatlogs));
        }
    }
}