using AliasPro.API.Permissions;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Models;
using AliasPro.Players.Types;
using AliasPro.Utilities;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationBanEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationBanMessageEvent;

		private const int BAN_18_HOURS = 3;
		private const int BAN_7_DAYS = 4;
		private const int BAN_30_DAYS_STEP_1 = 5;
		private const int BAN_30_DAYS_STEP_2 = 7;
		private const int BAN_100_YEARS = 6;
		private const int BAN_AVATAR_ONLY_100_YEARS = 106;

		private readonly IPlayerController _playerController;
		private readonly IPermissionsController _permissionsController;

		public ModerationBanEvent(
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
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_player_ban"))
				return;

			int playerId = message.ReadInt();

			IPlayer targetPlayer = await _playerController.GetPlayerAsync((uint)playerId);
			if (targetPlayer == null)
				return;

            if (targetPlayer.Rank >= session.Player.Rank) 
				return;

			string reason = message.ReadString();
			int topicId = message.ReadInt();
			int banType = message.ReadInt();
			message.ReadBoolean(); //dunno?

			int duration = (int)UnixTimestamp.Now;
			switch (banType)
			{
				case BAN_18_HOURS:
					duration += 18 * 60 * 60;
					break;
				case BAN_7_DAYS:
					duration += 7 * 24 * 60 * 60;
					break;
				case BAN_30_DAYS_STEP_1:
				case BAN_30_DAYS_STEP_2:
					duration += 30 * 24 * 60 * 60;
					break;
				case BAN_100_YEARS:
				case BAN_AVATAR_ONLY_100_YEARS:
					duration = int.MaxValue;
					break;
			}

			if (duration <= 0) return;

			IPlayerSanction sanction = new PlayerSanction(
				SanctionType.BAN, 
				duration,
				reason,
				topicId);

			await _playerController.AddPlayerSanction(targetPlayer.Id, sanction);

			if (targetPlayer.Session != null)
			{
				targetPlayer.Session.Disconnect();
				targetPlayer.Sanction.AddSanction(sanction);
			}
        }
    }
}