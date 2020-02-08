using AliasPro.API.Messenger;
using AliasPro.API.Permissions;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Moderation.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationKickEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationKickMessageEvent;

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

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
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