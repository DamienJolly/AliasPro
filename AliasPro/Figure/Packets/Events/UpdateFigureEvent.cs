using AliasPro.API.Achievements;
using AliasPro.API.Figure;
using AliasPro.API.Messenger;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Sessions.Models;
using AliasPro.Figure.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Types;

namespace AliasPro.Figure.Packets.Events
{
    public class UpdateFigureEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UpdateFigureMessageEvent;
       
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

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
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
                await session.CurrentRoom.SendAsync(new UpdateEntityDataComposer(session.Entity));
            }

            if (session.Player.Messenger != null)
                await _messengerController.UpdateStatusAsync(session.Player, session.Player.Messenger.Friends);

			_achievementController.ProgressAchievement(session.Player, "AvatarLooks");
		}
    }
}
