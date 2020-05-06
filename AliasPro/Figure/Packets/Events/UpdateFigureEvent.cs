using AliasPro.API.Figure;
using AliasPro.API.Messenger;
using AliasPro.API.Players;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Figure.Packets.Composers;
using AliasPro.Game.Achievements;
using AliasPro.Game.Achievements.Models;
using AliasPro.Players.Types;
using System.Threading.Tasks;

namespace AliasPro.Figure.Packets.Events
{
    public class UpdateFigureEvent : IMessageEvent
    {
        public short Header => Incoming.UpdateFigureMessageEvent;
       
        private readonly IPlayerController _playerController;
        private readonly IFigureController _figureController;
        private readonly IMessengerController _messengerController;
		private readonly AchievementController achievementController;

        public UpdateFigureEvent(
            IFigureController figureController, 
            IPlayerController playerController, 
            IMessengerController messengerController,
            AchievementController achievementController)
        {
            _figureController = figureController;
            _playerController = playerController;
            _messengerController = messengerController;
			this.achievementController = achievementController;
		}

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            PlayerGender gender = PlayerGender.MALE;

            switch (message.ReadString().ToLower())
            {
                case "m": gender = PlayerGender.MALE; break;
                case "f": gender = PlayerGender.FEMALE; break;
            }

            string figure = message.ReadString().ToLower();

            if (figure.Length == 0 || 
                figure == session.Player.Figure) 
                return;

            if (!_figureController.ValidateFigure(figure, gender)) 
                return;

            session.Player.Figure = figure;
            session.Player.Gender = gender;
            await session.SendPacketAsync(new UpdateFigureComposer(session.Player.Figure, session.Player.Gender));

            if (session.CurrentRoom != null &&
                session.Entity != null)
            {
                session.Entity.Figure = session.Player.Figure;
                session.Entity.Gender = session.Player.Gender;
                await session.CurrentRoom.SendPacketAsync(new UpdateEntityDataComposer(session.Entity));
            }

            if (session.Player.Messenger != null)
                await _messengerController.UpdateStatusAsync(session.Player, session.Player.Messenger.Friends);

            if (achievementController.TryGetAchievement("AvatarLooks", out AchievementData achievement))
                session.Player.Achievement.ProgressAchievement(session.Player, achievement);
        }
    }
}
