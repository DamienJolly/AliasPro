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
    public class ModerationTradeLockEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationTradeLockMessageEvent;
        
		private readonly IPlayerController _playerController;
		private readonly IPermissionsController _permissionsController;

		public ModerationTradeLockEvent(
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
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_player_tradelock"))
				return;

			int playerId = clientPacket.ReadInt();
			string message = clientPacket.ReadString();

			if (!_playerController.TryGetPlayer((uint)playerId, out IPlayer player))
				return;

            if (player.Session == null)
                return;

			//todo: code trade lock

            await player.Session.SendPacketAsync(new ModerationIssueHandledComposer(message));
        }
    }
}