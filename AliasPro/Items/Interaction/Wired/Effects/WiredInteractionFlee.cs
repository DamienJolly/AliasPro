﻿using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionFlee : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.FLEE;

        public WiredInteractionFlee(IItem item)
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
            IRoomPosition newPos =
                new RoomPosition(position.X, position.Y, position.Z);

            BaseEntity target = null;
            double shortest = 100.0D;

            foreach (BaseEntity entity in Room.Entities.Entities)
            {
                double distance = entity.Position.Distance(position);
                if (distance <= shortest)
                {
                    target = entity;
                    shortest = distance;
                }
            }

            if (target != null)
            {
                if (target.Position.X == position.X)
                {
                    if (position.Y < target.Position.Y) newPos.Y--;
                    else newPos.Y++;
                }
                else if (target.Position.Y == position.Y)
                {
                    if (position.X < target.Position.X) newPos.X--;
                    else newPos.X++;
                }
                else if (target.Position.X - position.X > target.Position.Y - position.Y)
                {
                    if (target.Position.X - position.X > 0) newPos.X--;
                    else newPos.X++;
                }
                else
                {
                    if (target.Position.Y - position.Y > 0) newPos.Y--;
                    else newPos.Y++;
                }
            } 
            else
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
    }
}
