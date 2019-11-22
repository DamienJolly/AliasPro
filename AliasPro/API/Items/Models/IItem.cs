using AliasPro.API.Items.Interaction;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Protocol;

namespace AliasPro.API.Items.Models
{
    public interface IItem
    {
        void ComposeFloorItem(ServerPacket serverPacket);
        void ComposeWallItem(ServerPacket serverPacket);

        uint Id { get; set; }
        uint ItemId { get; set; }
        uint PlayerId { get; set; }
        string PlayerUsername { get; set; }
        uint RoomId { get; set; }
        int Rotation { get; set; }
        int Mode { get; set; }
        string ExtraData { get; set; }
        IRoomPosition Position { get; set; }
        IItemData ItemData { get; set; }
        BaseEntity InteractingPlayer { get; set; }
		BaseEntity InteractingPlayerTwo { get; set; }

		IRoom CurrentRoom { get; set; }

        IItemInteractor Interaction { get; }
        IWiredInteractor WiredInteraction { get; }
    }
}
