using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Achievements.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Game.Achievements.Packets.Events
{
	public class RequestAchievementsEvent : IMessageEvent
	{
		public short Header => Incoming.RequestAchievementsMessageEvent;

		private readonly AchievementController achievementController;

		public RequestAchievementsEvent(
			AchievementController achievementController)
		{
			this.achievementController = achievementController;
		}

		public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
			if (session.Player == null)
				return;

			await session.SendPacketAsync(new AchievementListComposer(
				session.Player, 
				achievementController.Achievements));
		}
    }
}
