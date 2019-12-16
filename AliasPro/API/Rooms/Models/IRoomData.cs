using AliasPro.API.Groups.Models;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.API.Rooms.Models
{
    public interface IRoomData
    {
        void Compose(ServerPacket serverPacket);

        uint Id { get; set; }
        int OwnerId { get; set; }
        string OwnerName { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Password { get; set; }
        string ModelName { get; set; }
        int UsersNow { get; set; }
        int MaxUsers { get; set; }
        int CategoryId { get; set; }
        int TradeType { get; set; }
        int DoorState { get; set; }
        int Score { get; set; }
        IList<string> Tags { get; set; }
		IGroup Group { get; set; }
        bool IsPromoted { get; }

        IRoomModel RoomModel { get; set; }
        IRoomSettings Settings { get; set; }
        IRoomPromotion Promotion { get; set; }
    }
}
