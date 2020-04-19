using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Rooms.Cycles
{
    public class RoomCycle
    {
        private readonly IRoom _room;

        public RoomCycle(IRoom room)
        {
            _room = room;
        }

        public async void Cycle()
        {
            try
            {
                foreach (BaseEntity entity in _room.Entities.Entities)
                    entity.RoomEntityCycle.Cycle();

                foreach (IItem item in _room.Items.Items)
                    item.Interaction.OnCycle();

                if (_room.RollerCycle <= 0)
                {
                    _room.RollerCycle = _room.RollerSpeed;

                    ICollection<int> itemsRolled = new List<int>();
                    ICollection<int> entitiesRolled = new List<int>();

                    foreach (IItem item in _room.Items.GetItemsByType(ItemInteractionType.ROLLER))
                    {
                        if (!item.CurrentRoom.RoomGrid.TryGetRoomTile(item.Position.X, item.Position.Y, out IRoomTile rollerTile))
                            return;

                        IRoomPosition newPos = HandleMovement(item.Rotation, item.Position);

                        if (!item.CurrentRoom.RoomGrid.TryGetRoomTile(newPos.X, newPos.Y, out IRoomTile roomTile)) continue;

                        foreach (IItem rollerItem in rollerTile.Items.ToList())
                        {
                            if (rollerItem.Id == item.Id) continue;

                            if (itemsRolled.Contains((int)rollerItem.Id)) continue;

                            item.CurrentRoom.Items.TriggerWired(WiredInteractionType.COLLISION, newPos);

                            if (!item.CurrentRoom.RoomGrid.CanRollAt(newPos.X, newPos.Y, rollerItem))
                                continue;

                            newPos.Z = roomTile.Height;
                            await item.CurrentRoom.SendPacketAsync(new FloorItemOnRollerComposer(rollerItem, newPos, item.Id));

                            itemsRolled.Add((int)rollerItem.Id);
                            item.CurrentRoom.RoomGrid.RemoveItem(rollerItem);
                            rollerItem.Position = newPos;
                            item.CurrentRoom.RoomGrid.AddItem(rollerItem);
                        }

                        foreach (BaseEntity entity in rollerTile.Entities.ToList())
                        {
                            if (entity.NextPosition != entity.Position) continue;

                            if (entitiesRolled.Contains(entity.Id)) continue;

                            if (!roomTile.IsValidTile(entity, true))
                                continue;

                            newPos.Z = roomTile.Height;
                            await item.CurrentRoom.SendPacketAsync(new EntityOnRollerComposer(entity, newPos, item.Id));

                            entitiesRolled.Add(entity.Id);
                            item.CurrentRoom.RoomGrid.RemoveEntity(entity);
                            entity.NextPosition = newPos;
                            entity.GoalPosition = newPos;
                            item.CurrentRoom.RoomGrid.AddEntity(entity);
                        }
                    }
                }
                else
                    _room.RollerCycle--;

                if (_room.Entities.Entities.Count <= 0)
                    _room.IdleTimer++;
                else
                {
                    foreach (BaseEntity entity in _room.Entities.Entities)
                    {
                        entity.NeedsUpdate = false;
                        await _room.SendPacketAsync(new EntityUpdateComposer(entity));
                    }
                }
            }
            catch { }
        }

        private IRoomPosition HandleMovement(int rotation, IRoomPosition position)
        {
            IRoomPosition newPos =
                new RoomPosition(position.X, position.Y, position.Z);

            switch (rotation)
            {
                case 0: newPos.Y--; break;
                case 2: newPos.X++; break;
                case 4: newPos.Y++; break;
                case 6: newPos.X--; break;
            }

            return newPos;
        }
    }
}
