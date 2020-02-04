using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class FloorPlanBlockedTilesComposer : IMessageComposer
    {
        private readonly ICollection<IRoomTile> _lockedTiles;

        public FloorPlanBlockedTilesComposer(ICollection<IRoomTile> lockedTiles)
        {
            _lockedTiles = lockedTiles;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.FloorPlanBlockedTilesMessageComposer);
            message.WriteInt(_lockedTiles.Count);
            foreach (IRoomTile tile in _lockedTiles)
            {
                message.WriteInt(tile.Position.X);
                message.WriteInt(tile.Position.Y);
            }
            return message;
        }
    }
}
