using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Entities;
using AliasPro.Rooms.Components;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Tasks;
using System.Threading.Tasks;

namespace AliasPro.API.Rooms.Models
{
    public interface IRoom : IRoomData
    {
        EntitiesComponent Entities { get; set; }
        ItemsComponent Items { get; set; }
        RightsComponent Rights { get; set; }
        GameComponent Game { get; set; }

        RoomGrid RoomGrid { get; set; }
        RoomCycleTask RoomTask { get; set; }

        Task AddEntity(BaseEntity entity);
        Task RemoveEntity(BaseEntity entity);
        void OnChat(string text, int colour, BaseEntity entity);
        Task SendAsync(IPacketComposer packet);
        void Dispose();
    }
}
