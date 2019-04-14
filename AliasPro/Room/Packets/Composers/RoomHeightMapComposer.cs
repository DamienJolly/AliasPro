using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class HeightMapComposer : IPacketComposer
    {
        private readonly IRoomModel _roomModel;

        public HeightMapComposer(IRoomModel roomModel)
        {
            _roomModel = roomModel;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.HeightMapMessageComposer);
            message.WriteInt(_roomModel.MapSizeX);
            message.WriteInt(_roomModel.MapSizeX * _roomModel.MapSizeY);

            for (int y = 0; y < _roomModel.MapSizeY; y++)
            {
                for (int x = 0; x < _roomModel.MapSizeX; x++)
                {
                    if (_roomModel.GetTileState(x, y))
                    {
                        message.WriteShort((short)(_roomModel.GetHeight(x, y) * 256));
                    }
                    else
                    {
                        message.WriteShort(-1);
                    }
                }
            }
            return message;
        }
    }
}
