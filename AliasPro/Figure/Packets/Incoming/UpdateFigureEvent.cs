using System.Threading.Tasks;

namespace AliasPro.Figure.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Player;

    public class UpdateFigureEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UpdateFigureMessageEvent;
       
        private readonly IPlayerController _playerController;
        private readonly IFigureController _figureController;

        public UpdateFigureEvent(IFigureController figureController, IPlayerController playerController)
        {
            _figureController = figureController;
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: use enum instead
            string gender = clientPacket.ReadString().ToUpper();
            if (gender != "M" && gender != "F") return;

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
                await _playerController.UpdateStatus(session.Player, session.Player.Messenger.Friends);
        }
    }
}
