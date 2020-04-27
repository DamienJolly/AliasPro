using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Interaction;
using AliasPro.Items.Interaction.Wired;

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
        string WiredData { get; set; }
        IRoomPosition Position { get; set; }
        string WallCord { get; set; }
        IItemData ItemData { get; set; }
        BaseEntity InteractingPlayer { get; set; }
		BaseEntity InteractingPlayerTwo { get; set; }
        IRoom CurrentRoom { get; set; }

        ItemInteraction Interaction { get; set; }
        WiredInteraction WiredInteraction { get; set; }
    }
}
