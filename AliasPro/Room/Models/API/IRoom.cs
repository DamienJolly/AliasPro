using System.Collections.Generic;

namespace AliasPro.Room.Models
{
    using Entities;
    using Gamemap;
    using Sessions;

    public interface IRoom
    {
        RoomMap RoomMap { get; set; }
        IRoomData RoomData { get; set; }
        IRoomModel RoomModel { get; set; }
        IDictionary<int, BaseEntity> Entities { get; }
        bool CycleActive { get; }
        void OnChat(string text, int colour, BaseEntity entity);
        void LeaveRoom(ISession session);
        void SetupRoomCycle();
        void StopRoomCycle();
        void AddEntity(BaseEntity entity);
    }
}
