using AliasPro.API.Messenger;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestRoomLoadDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomLoadDataMessageEvent;

        private readonly IMessengerController _messengerController;

        public RequestRoomLoadDataEvent(
            IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            System.Console.WriteLine("hi");

            IRoom room = session.CurrentRoom;
            if (room == null) return;

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
