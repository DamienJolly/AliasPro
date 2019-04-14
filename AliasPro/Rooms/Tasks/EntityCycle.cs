using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Tasks;

namespace AliasPro.Rooms.Tasks
{
    public class EntityCycle : ITask
    {
        private readonly IRoom _room;

        public EntityCycle(IRoom room)
        {
            _room = room;
        }

        public async void Run()
        {
            try
            {
                foreach (BaseEntity entity in _room.Entities.Entities)
                {
                    await TaskHandler.RunTaskAsync(new WalkCycle(_room, entity));
                    entity.Cycle();
                }

                await _room.SendAsync(new EntityUpdateComposer(_room.Entities.Entities));
            }
            catch
            {
                // room crashed
            }
        }
    }
}
