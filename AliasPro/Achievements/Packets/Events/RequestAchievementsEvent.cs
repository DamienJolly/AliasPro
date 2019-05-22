using AliasPro.Achievements.Packets.Composers;
using AliasPro.API.Achievements;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Achievements.Packets.Events
{
	public class RequestAchievementsEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.RequestAchievementsMessageEvent;

		private readonly IAchievementController _achievementController;

		public RequestAchievementsEvent(
			IAchievementController achievementController)
		{
			_achievementController = achievementController;
		}

		public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
			await session.SendPacketAsync(new AchievementListComposer(
				session.Player, 
				_achievementController.Achievements));
		}
    }
}
