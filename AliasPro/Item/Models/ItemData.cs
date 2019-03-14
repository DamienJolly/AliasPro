using System.Data.Common;

namespace AliasPro.Item.Models
{
    using Database;

    internal class ItemData : IItemData
    {
        internal ItemData(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            SpriteId = reader.ReadData<uint>("sprite_id");
            Name = reader.ReadData<string>("item_name");
            Length = reader.ReadData<int>("length");
            Width = reader.ReadData<int>("width");
            Height = reader.ReadData<double>("height");
            CanSit = reader.ReadData<bool>("can_sit");
            CanLay = reader.ReadData<bool>("can_lay");
            ExtraData = reader.ReadData<string>("extra_data");
            Type = reader.ReadData<string>("type");
            Modes = reader.ReadData<int>("modes");
            InteractionType = GetItemInteractor(
                reader.ReadData<string>("interaction_type"));
            CanWalk = reader.ReadData<bool>("can_walk");
            CanStack = reader.ReadData<bool>("can_stack");
        }

        private static ItemInteraction GetItemInteractor(string interaction)
        {
            switch (interaction)
            {
                case "wired_trigger": return ItemInteraction.WIRED_TRIGGER;
                case "wired_effect": return ItemInteraction.WIRED_EFFECT;
                case "wired_condition": return ItemInteraction.WIRED_CONDITION;
                case "vending": return ItemInteraction.VENDING_MACHINE;
                case "default": default: return ItemInteraction.DEFAULT;
            }
        }

        public uint Id { get; }
        public uint SpriteId { get; }
        public string Name { get; }
        public int Length { get; }
        public int Width { get; }
        public double Height { get; }
        public bool CanSit { get; }
        public bool CanLay { get; }
        public string ExtraData { get; }
        public string Type { get; }
        public int Modes { get; }
        public ItemInteraction InteractionType { get; }
        public bool CanWalk { get; }
        public bool CanStack { get; }
    }

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
        ItemInteraction InteractionType { get; }
        bool CanWalk { get; }
        bool CanStack { get; }
    }
}
