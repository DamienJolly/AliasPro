using AliasPro.API.Items.Interaction;
using AliasPro.Network.Protocol;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Models;
using AliasPro.Room.Models.Entities;

namespace AliasPro.API.Items.Models
{
    public interface IItem
    {
        void ComposeFloorItem(ServerPacket serverPacket);
        void ComposeWallItem(ServerPacket serverPacket);

        uint Id { get; set; }
        uint ItemId { get; }
        uint PlayerId { get; }
        string PlayerUsername { get; }
        uint RoomId { get; set; }
        int Rotation { get; set; }
        int Mode { get; set; }
        string ExtraData { get; set; }
        Position Position { get; set; }
        IItemData ItemData { get; set; }
        BaseEntity InteractingPlayer { get; set; }

        IRoom CurrentRoom { get; set; }

        IItemInteractor Interaction { get; }
        IWiredInteractor WiredInteraction { get; }
    }
}
