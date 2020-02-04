using AliasPro.API.Messenger;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestRoomLoadDataEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.RequestRoomLoadDataMessageEvent;

        private readonly IMessengerController _messengerController;

        public RequestRoomLoadDataEvent(
            IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            System.Console.WriteLine("hi");

            IRoom room = session.CurrentRoom;
            if (room == null) 
                return;

            await session.SendPacketAsync(new RoomModelComposer(room.RoomModel.Id, room.Id));
            await session.SendPacketAsync(new RoomScoreComposer(room.Score));

            if (!room.WallPaint.Equals("0.0"))
                await session.SendPacketAsync(new RoomPaintComposer("wallpaper", room.WallPaint));

            if (!room.FloorPaint.Equals("0.0"))
                await session.SendPacketAsync(new RoomPaintComposer("floor", room.FloorPaint));

            await session.SendPacketAsync(new RoomPaintComposer("landscape", room.BackgroundPaint));

            if (session.Player.Messenger != null)
                await _messengerController.UpdateStatusAsync(session.Player, session.Player.Messenger.Friends);
        }
    }
}
