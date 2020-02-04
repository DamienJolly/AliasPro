using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class HeightMapComposer : IMessageComposer
    {
        private readonly IRoom _room;

        public HeightMapComposer(IRoom room)
        {
            _room = room;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.HeightMapMessageComposer);
            message.WriteInt(_room.RoomGrid.MapSizeX);
            message.WriteInt(_room.RoomGrid.MapSizeX * _room.RoomGrid.MapSizeY);

            for (int y = 0; y < _room.RoomGrid.MapSizeY; y++)
            {
                for (int x = 0; x < _room.RoomGrid.MapSizeX; x++)
                {
                    if (_room.RoomGrid.TryGetRoomTile(x, y, out IRoomTile roomTile))
                    {
                        message.WriteShort(roomTile.RelativeHeight);
                    }
                    else
                    {
                        message.WriteShort(short.MaxValue);
                    }
                }
            }
            return message;
        }
    }
}
