using AliasPro.API.Messenger;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Figure.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Players.Packets.Events
{
    public class UserChangeMottoEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserChangeMottoMessageEvent;

        private readonly IMessengerController _messengerController;

        public UserChangeMottoEvent(
            IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string motto = clientPacket.ReadString();

            session.Player.Motto = motto;

            if (session.CurrentRoom != null &&
                session.Entity != null)
            {
                session.Entity.Motto = session.Player.Motto;
                await session.CurrentRoom.SendAsync(new UpdateEntityDataComposer(session.Entity));
            }

            if (session.Player.Messenger != null)
                await _messengerController.UpdateStatusAsync(session.Player, session.Player.Messenger.Friends);
        }
    }
}
