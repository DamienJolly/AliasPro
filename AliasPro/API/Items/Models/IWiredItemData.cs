using AliasPro.Room.Gamemap;

namespace AliasPro.API.Items.Models
{
    public interface IWiredItemData
    {
        uint ItemId { get; set; }
        Position Position { get; set; }
        int Mode { get; set; }
        int Rotation { get; set; }
        int MovementDirection { get; set; }
    }
}
