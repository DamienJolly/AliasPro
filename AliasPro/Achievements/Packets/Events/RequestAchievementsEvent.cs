using AliasPro.Achievements.Packets.Composers;
using AliasPro.API.Achievements;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Achievements.Packets.Events
{
	public class RequestAchievementsEvent : IMessageEvent
	{
		public short Header => Incoming.RequestAchievementsMessageEvent;

		private readonly IAchievementController _achievementController;

		public RequestAchievementsEvent(
			IAchievementController achievementController)
		{
			_achievementController = achievementController;
		}

		public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
			if (session.Player == null)
				return;

			await session.SendPacketAsync(new AchievementListComposer(
				session.Player, 
				_achievementController.Achievements));
		}
    }
}
