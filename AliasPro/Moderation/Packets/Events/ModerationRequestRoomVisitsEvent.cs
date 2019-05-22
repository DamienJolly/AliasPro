using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Permissions;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRequestRoomVisitsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationRequestRoomVisitsMessageEvent;
        
		private readonly IPlayerController _playerController;
		private readonly IPermissionsController _permissionsController;

		public ModerationRequestRoomVisitsEvent(
			IPlayerController playerController,
			IPermissionsController permissionsController)
		{
			_playerController = playerController;
			_permissionsController = permissionsController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_player_info"))
				return;

			int playerId = clientPacket.ReadInt();

            IPlayerData player = await _playerController.GetPlayerDataAsync((uint)playerId);
            if (player == null)
                return;

            await session.SendPacketAsync(new ModerationUserRoomVisitsComposer(player,
                await _playerController.GetPlayerRoomVisitsAsync(player.Id)));
        }
    }
}