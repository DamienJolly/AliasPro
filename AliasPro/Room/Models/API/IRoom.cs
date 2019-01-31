using System.Collections.Generic;

namespace AliasPro.Room.Models
{
    using Entities;

    public interface IRoom
    {
        IRoomData RoomData { get; set; }
        IRoomModel RoomModel { get; set; }
        IDictionary<int, BaseEntity> Entities { get; }
        void AddEntity(BaseEntity entity);
    }
}
