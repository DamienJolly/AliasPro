using AliasPro.API.Rooms.Entities;
using AliasPro.API.Tasks;

namespace AliasPro.Rooms.Tasks
{
    public class LookToTask : ITask
    {
        private readonly BaseEntity _entity;

        public LookToTask(BaseEntity entity)
        {
            _entity = entity;
        }

        public void Run()
        {
            if (_entity != null)
                _entity.SetRotation(_entity.BodyRotation);
        }
    }
}
