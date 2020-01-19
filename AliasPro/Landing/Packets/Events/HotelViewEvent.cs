using AliasPro.API.Messenger;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Landing.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Landing.Packets.Events
{
    public class HotelViewEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.HotelViewMessageEvent;

        private readonly IMessengerController _messengerController;

        public HotelViewEvent(
            IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
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