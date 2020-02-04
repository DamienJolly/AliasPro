using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class UpdateStackHeightComposer : IMessageComposer
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

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UpdateStackHeightMessageComposer);
            message.WriteByte((byte)_tiles.Count);
            foreach (IRoomTile tile in _tiles)
            {
                message.WriteByte((byte)tile.Position.X);
                message.WriteByte((byte)tile.Position.Y);
                message.WriteShort(tile.RelativeHeight);
            }
            return message;
        }
    }
}
