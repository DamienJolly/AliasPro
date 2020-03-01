using AliasPro.API.Rooms.Models;

namespace AliasPro.API.Items.Models
{
    public interface IWiredItemData
    {
        uint ItemId { get; set; }
        IRoomPosition Position { get; set; }
        string ExtraData { get; set; }
        int Rotation { get; set; }
        int MovementDirection { get; set; }
    }
}
