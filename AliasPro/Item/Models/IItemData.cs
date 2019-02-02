namespace AliasPro.Item.Models
{
    public interface IItemData
    {
        uint Id { get; }
        uint SpriteId { get; }
        string Name { get; }
        int Length { get; }
        int Width { get; }
        double Height { get; }
        bool CanSit { get; }
        bool CanLay { get; }
        string ExtraData { get; }
        string Type { get; }
        int Modes { get; }
        string Interaction { get; }
        bool CanWalk { get; }
        bool CanStack { get; }
    }
}
