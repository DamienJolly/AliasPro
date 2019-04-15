using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Tasks;

namespace AliasPro.Rooms.Tasks
{
    public class EntitiesTask : ITask
    {
        private readonly IRoom _room;

        public EntitiesTask(IRoom room)
        {
            _room = room;
        }

        public async void Run()
        {
            foreach (BaseEntity entity in _room.Entities.Entities)
            {
                entity.Cycle();
                await TaskHandler.RunTaskAsync(new WalkTask(_room, entity));
            }

            await _room.SendAsync(new EntityUpdateComposer(_room.Entities.Entities));
        }
    }
}
