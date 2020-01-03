using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class UpdateStackHeightComposer : IPacketComposer
    {
        private readonly ICollection<IRoomTile> _tiles;

        public UpdateStackHeightComposer(ICollection<IRoomTile> tiles)
        {
            _tiles = tiles;
        }

        public UpdateStackHeightComposer(IRoomTile tile)
        {
            _tiles = new List<IRoomTile> { tile };
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UpdateStackHeightMessageComposer);
            message.WriteByte(_tiles.Count);
            foreach (IRoomTile tile in _tiles)
            {
                System.Console.WriteLine(tile.Position.X + " : " + tile.Position.Y);
                message.WriteByte(tile.Position.X);
                message.WriteByte(tile.Position.Y);
                message.WriteShort(tile.RelativeHeight);
            }
            return message;
        }
    }
}
