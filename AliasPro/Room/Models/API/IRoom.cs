using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Room.Models
{
    using Network.Events;
    using Entities;
    using Gamemap;
    using Sessions;

    public interface IRoom
    {
        RoomMap RoomMap { get; set; }
        IRoomData RoomData { get; set; }
        IRoomModel RoomModel { get; set; }
        IDictionary<int, BaseEntity> Entities { get; }
        void OnChat(string text, int colour, BaseEntity entity);
        void LeaveRoom(ISession session);
        Task SendAsync(IPacketComposer serverPacket);
        void AddEntity(BaseEntity entity);
    }
}
