using AliasPro.API.Messenger;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Landing.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Landing.Packets.Events
{
    public class HotelViewEvent : IMessageEvent
    {
        public short Header => Incoming.HotelViewMessageEvent;

        private readonly IMessengerController _messengerController;

        public HotelViewEvent(
            IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            if (session.CurrentRoom == null)
                return;
            
            if (session.Entity != null)
                await session.CurrentRoom.RemoveEntity(session.Entity);
            else
                await session.SendPacketAsync(new HotelViewComposer());

            session.CurrentRoom = null;

            if (session.Player.Messenger != null)
                await _messengerController.UpdateStatusAsync(session.Player, session.Player.Messenger.Friends);
        }
    }
}