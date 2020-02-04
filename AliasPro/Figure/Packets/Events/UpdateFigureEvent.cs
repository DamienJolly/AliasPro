using AliasPro.API.Achievements;
using AliasPro.API.Figure;
using AliasPro.API.Messenger;
using AliasPro.API.Players;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Figure.Packets.Composers;
using AliasPro.Players.Types;
using System.Threading.Tasks;

namespace AliasPro.Figure.Packets.Events
{
    public class UpdateFigureEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.UpdateFigureMessageEvent;
       
        private readonly IPlayerController _playerController;
        private readonly IFigureController _figureController;
        private readonly IMessengerController _messengerController;
		private readonly IAchievementController _achievementController;

        public UpdateFigureEvent(
            IFigureController figureController, 
            IPlayerController playerController, 
            IMessengerController messengerController,
			IAchievementController achievementController)
        {
            _figureController = figureController;
            _playerController = playerController;
            _messengerController = messengerController;
			_achievementController = achievementController;
		}

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            PlayerGender gender = PlayerGender.MALE;

            switch (clientPacket.ReadString().ToLower())
            {
                case "m": gender = PlayerGender.MALE; break;
                case "f": gender = PlayerGender.FEMALE; break;
            }

            string figure = clientPacket.ReadString().ToLower();

            if (figure.Length == 0 || 
                figure == session.Player.Figure) return;

            if (!_figureController.ValidateFigure(figure, gender)) return;

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

			_achievementController.ProgressAchievement(session.Player, "AvatarLooks");
		}
    }
}
