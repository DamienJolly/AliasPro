using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;
using Pathfinding;
using Pathfinding.Models;
using Pathfinding.Types;
using System.Collections.Generic;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionChase : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.CHASE;

        public WiredInteractionChase(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            foreach (WiredItemData itemData in WiredData.Items.Values)
            {
                if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item)) 
                    continue;

                IRoomPosition newPos = HandlePosition(item.Position);

                Room.Items.TriggerWired(WiredInteractionType.COLLISION, newPos);

                if (!Room.RoomGrid.TryGetRoomTile(newPos.X, newPos.Y, out IRoomTile roomTile) ||
                    !Room.RoomGrid.CanRollAt(newPos.X, newPos.Y, item))
                    continue;

                newPos.Z = roomTile.Height;
                Room.SendPacketAsync(new FloorItemOnRollerComposer(item, newPos, 0));

                Room.RoomGrid.RemoveItem(item);
                item.Position = newPos;
                Room.RoomGrid.AddItem(item);
            }
            return true;
        }

        private IRoomPosition HandlePosition(IRoomPosition position)
        {

            if (!TryGetClosestPosition(position, out IRoomPosition newPos))
            {
                int randomNum = Randomness.RandomNumber(1, 4);
                switch (randomNum)
                {
                    case 1: newPos.X += 1; break;
                    case 2: newPos.X -= 1; break;
                    case 3: newPos.Y += 1; break;
                    case 4: newPos.Y -= 1; break;
                }
            }

            return newPos;
        }

        private bool TryGetClosestPosition(IRoomPosition position, out IRoomPosition newPosition)
        {
            newPosition = new RoomPosition(position.X, position.Y, position.Z);
            IList<Position> closestPath = null;
            
            foreach (BaseEntity entity in Room.Entities.Entities)
            {
                IList<Position> pathToItem = Pathfinder.FindPath(
                    Room.RoomGrid,
                    new Position(entity.Position.X, entity.Position.Y),
                    new Position(position.X, position.Y),
                    DiagonalMovement.ONE_WALKABLE);

                if (pathToItem.Count <= closestPath.Count)
                    closestPath = pathToItem;
            }

            if (closestPath != null)
                newPosition = new RoomPosition(closestPath[closestPath.Count - 1]);

            return closestPath == null ? false : true;
        }
    }
}
