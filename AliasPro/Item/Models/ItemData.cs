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
            ExtraData = reader.ReadData<string>("extra_data");
            Type = reader.ReadData<string>("type");
            Modes = reader.ReadData<int>("modes");
            CanWalk = reader.ReadData<bool>("can_walk");
            CanStack = reader.ReadData<bool>("can_stack");

            InteractionType = GetItemInteractor(
                reader.ReadData<string>("interaction_type"));
            WiredInteractionType = WiredInteraction.DEFAULT;

            if (IsWired && int.TryParse(ExtraData, out int wiredId))
                WiredInteractionType = (WiredInteraction)wiredId;
        }

        private static ItemInteraction GetItemInteractor(string interaction)
        {
            switch (interaction)
            {
                case "wired_trigger": return ItemInteraction.WIRED_TRIGGER;
                case "wired_effect": return ItemInteraction.WIRED_EFFECT;
                case "wired_condition": return ItemInteraction.WIRED_CONDITION;
                case "game_timer": return ItemInteraction.GAME_TIMER;
                case "vending": return ItemInteraction.VENDING_MACHINE;
                case "bed": return ItemInteraction.BED;
                case "chair": return ItemInteraction.CHAIR;
                case "default": default: return ItemInteraction.DEFAULT;
            }
        }

        public uint Id { get; }
        public uint SpriteId { get; }
        public string Name { get; }
        public int Length { get; }
        public int Width { get; }
        public double Height { get; }
        public string ExtraData { get; }
        public string Type { get; }
        public int Modes { get; }
        public bool CanWalk { get; }
        public bool CanStack { get; }
        
        public ItemInteraction InteractionType { get; }
        public WiredInteraction WiredInteractionType { get; }

        public bool IsWired =>
            InteractionType == ItemInteraction.WIRED_CONDITION ||
            InteractionType == ItemInteraction.WIRED_TRIGGER ||
            InteractionType == ItemInteraction.WIRED_EFFECT;
    }

    public interface IItemData
    {
        uint Id { get; }
        uint SpriteId { get; }
        string Name { get; }
        int Length { get; }
        int Width { get; }
        double Height { get; }
        string ExtraData { get; }
        string Type { get; }
        int Modes { get; }
        bool CanWalk { get; }
        bool CanStack { get; }
        
        ItemInteraction InteractionType { get; }
        WiredInteraction WiredInteractionType { get; }

        bool IsWired { get; }
    }
}
