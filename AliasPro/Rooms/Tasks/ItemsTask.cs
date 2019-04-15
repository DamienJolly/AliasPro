using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;

namespace AliasPro.Rooms.Tasks
{
    public class ItemsTask : ITask
    {
        private readonly IRoom _room;

        public ItemsTask(IRoom room)
        {
            _room = room;
        }

        public void Run()
        {
            foreach (IItem item in _room.Items.Items)
            {
                item.Interaction.OnCycle();
            }
        }
    }
}
