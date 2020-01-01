using AliasPro.API.Messenger;
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
    public class ModerationKickEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationKickMessageEvent;

		private readonly IPlayerController _playerController;
		private readonly IPermissionsController _permissionsController;
		private readonly IMessengerController _messengerController;

		public ModerationKickEvent(
			IPlayerController playerController,
			IPermissionsController permissionsController,
			IMessengerController messengerController)
		{
			_playerController = playerController;
			_permissionsController = permissionsController;
			_messengerController = messengerController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_player_kick"))
				return;

			int playerId = clientPacket.ReadInt();
			string message = clientPacket.ReadString();

			if (!_playerController.TryGetPlayer((uint)playerId, out IPlayer player))
				return;

            if (player.Session == null || player.Session.CurrentRoom == null)
                return;

			await player.Session.CurrentRoom.RemoveEntity(player.Session.Entity);
			await player.Session.SendPacketAsync(new ModerationIssueHandledComposer(message));

			if (player.Messenger != null)
				await _messengerController.UpdateStatusAsync(player, player.Messenger.Friends);
		}
    }
}