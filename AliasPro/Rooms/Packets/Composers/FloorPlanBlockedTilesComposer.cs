using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class FloorPlanBlockedTilesComposer : IPacketComposer
    {
        private readonly ICollection<IRoomTile> _lockedTiles;

        public FloorPlanBlockedTilesComposer(ICollection<IRoomTile> lockedTiles)
        {
            _lockedTiles = lockedTiles;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.FloorPlanBlockedTilesMessageComposer);
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
