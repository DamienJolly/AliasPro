namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models;

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
