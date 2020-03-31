using AliasPro.API.Permissions;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Players.Models;
using AliasPro.Players.Types;
using AliasPro.Utilities;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationMuteEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationMuteMessageEvent;

		private readonly IPlayerController _playerController;
		private readonly IPermissionsController _permissionsController;

		public ModerationMuteEvent(
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
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_player_mute"))
				return;

			int playerId = message.ReadInt();
			string msg = message.ReadString();
			int topicId = message.ReadInt();

			IPlayer targetPlayer = await _playerController.GetPlayerAsync((uint)playerId);
			if (targetPlayer == null)
				return;

			IPlayerSanction sanction = new PlayerSanction(
				SanctionType.MUTE,
				(int)UnixTimestamp.Now + (60 * 60),
				msg,
				topicId);

			await _playerController.AddPlayerSanction(targetPlayer.Id, sanction);

			if (targetPlayer.Session != null)
			{
				targetPlayer.Sanction.AddSanction(sanction);
				await targetPlayer.Session.SendPacketAsync(new ModerationIssueHandledComposer(msg));
			}
        }
    }
}