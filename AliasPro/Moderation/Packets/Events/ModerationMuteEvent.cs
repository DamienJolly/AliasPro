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
    public class ModerationMuteEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationMuteMessageEvent;

		private readonly IPlayerController _playerController;
		private readonly IPermissionsController _permissionsController;

		public ModerationMuteEvent(
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
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_player_mute"))
				return;

			int playerId = clientPacket.ReadInt();
			string message = clientPacket.ReadString();

			if (!_playerController.TryGetPlayer((uint)playerId, out IPlayer player))
				return;

            if (player.Session == null)
                return;

			//todo: player muting

			await player.Session.SendPacketAsync(new ModerationIssueHandledComposer(message));
        }
    }
}