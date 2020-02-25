using AliasPro.API.Items.Interaction;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.API.Items.Models
{
    public interface IItem
    {
        void ComposeFloorItem(ServerMessage ServerMessage);
        void ComposeWallItem(ServerMessage ServerMessage);

        uint Id { get; set; }
        uint ItemId { get; set; }
        uint PlayerId { get; set; }
        string PlayerUsername { get; set; }
        uint RoomId { get; set; }
        int Rotation { get; set; }
        string ExtraData { get; set; }
        IRoomPosition Position { get; set; }
        string WallCord { get; set; }
        IItemData ItemData { get; set; }
        BaseEntity InteractingPlayer { get; set; }
		BaseEntity InteractingPlayerTwo { get; set; }

		IRoom CurrentRoom { get; set; }

        IItemInteractor Interaction { get; set; }
        IWiredInteractor WiredInteraction { get; set; }
    }
}
