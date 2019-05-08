using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;

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

                if (_room.Entities.Entities.Count <= 0)
                    _room.IdleTimer++;
                else
                    await _room.SendAsync(new EntityUpdateComposer(_room.Entities.Entities));
            }
            catch { }
        }
    }
}
