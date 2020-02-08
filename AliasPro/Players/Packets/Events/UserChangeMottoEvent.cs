using AliasPro.API.Messenger;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Figure.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class UserChangeMottoEvent : IMessageEvent
    {
        public short Header => Incoming.UserChangeMottoMessageEvent;

        private readonly IMessengerController _messengerController;

        public UserChangeMottoEvent(
            IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            string motto = message.ReadString();

            session.Player.Motto = motto;

            if (session.CurrentRoom != null &&
                session.Entity != null)
            {
                session.Entity.Motto = session.Player.Motto;
                await session.CurrentRoom.SendPacketAsync(new UpdateEntityDataComposer(session.Entity));
            }

            if (session.Player.Messenger != null)
                await _messengerController.UpdateStatusAsync(session.Player, session.Player.Messenger.Friends);
        }
    }
}
