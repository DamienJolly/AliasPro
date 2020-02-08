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
    public class ModerationTradeLockEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationTradeLockMessageEvent;
        
		private readonly IPlayerController _playerController;
		private readonly IPermissionsController _permissionsController;

		public ModerationTradeLockEvent(
			IPlayerController playerController,
			IPermissionsController permissionsController)
		{
			_playerController = playerController;
			_permissionsController = permissionsController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage message)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_player_tradelock"))
				return;

			int playerId = message.ReadInt();
			string msg = message.ReadString();

			if (!_playerController.TryGetPlayer((uint)playerId, out IPlayer player))
				return;

            if (player.Session == null)
                return;

			//todo: code trade lock

            await player.Session.SendPacketAsync(new ModerationIssueHandledComposer(msg));
        }
    }
}