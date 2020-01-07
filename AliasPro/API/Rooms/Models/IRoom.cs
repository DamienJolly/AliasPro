using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Entities;
using AliasPro.Rooms.Components;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Cycles;
using System.Threading.Tasks;
using AliasPro.API.Groups.Models;
using System.Collections.Generic;
using AliasPro.Rooms.Types;

namespace AliasPro.API.Rooms.Models
{
    public interface IRoom : IRoomData
    {
        EntitiesComponent Entities { get; set; }
        ItemsComponent Items { get; set; }
        RightsComponent Rights { get; set; }
        GameComponent Game { get; set; }
        MuteComponent Mute { get; set; }
		RoomGrid RoomGrid { get; set; }
        RoomCycle RoomCycle { get; set; }
        IList<string> WordFilter { get; set; }
        int IdleTimer { get; set; }
        int RollerSpeed { get; set; }
        int RollerCycle { get; set; }
        bool Loaded { get; set; }
		bool Muted { get; set; }


		void Cycle();
        Task AddEntity(BaseEntity entity);
		Task RemoveEntity(BaseEntity entity, bool notifyUser = true);
		void OnChat(string text, int colour, BaseEntity entity, RoomChatType chatType = RoomChatType.TALK);
		Task UpdateRoomGroup(IGroup group);
		Task SendAsync(IPacketComposer packet);
        void Dispose();
    }
}
