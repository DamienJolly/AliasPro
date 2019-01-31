namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;
    using Models;

    public class HeightMapComposer : ServerPacket
    {
        public HeightMapComposer(IRoomModel roomModel)
            : base(Outgoing.HeightMapMessageComposer)
        {
            WriteInt(roomModel.MapSizeX);
            WriteInt(roomModel.MapSizeX * roomModel.MapSizeY);

            for (int y = 0; y < roomModel.MapSizeY; y++)
            {
                for (int x = 0; x < roomModel.MapSizeX; x++)
                {
                    //Tile is valid.
                    if (roomModel.GetTileState(x, y))
                    {
                        WriteShort((short)(roomModel.GetHeight(x, y) * 256));
                    }
                    else
                    {
                        WriteShort(-1);
                    }
                }
            }
        }
    }
}
