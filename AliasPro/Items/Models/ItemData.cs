using AliasPro.API.Database;
using AliasPro.API.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Items.Utilities;
using System.Data.Common;

namespace AliasPro.Items.Models
{
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

            InteractionType = ItemInteractionUtility.GetItemInteractor(
                reader.ReadData<string>("interaction_type"));
            WiredInteractionType = WiredInteractionType.DEFAULT;

            if (IsWired && int.TryParse(ExtraData, out int wiredId))
                WiredInteractionType = (WiredInteractionType)wiredId;
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
        
        public ItemInteractionType InteractionType { get; }
        public WiredInteractionType WiredInteractionType { get; }

        public bool IsWired =>
            InteractionType == ItemInteractionType.WIRED_CONDITION ||
            InteractionType == ItemInteractionType.WIRED_TRIGGER ||
            InteractionType == ItemInteractionType.WIRED_EFFECT;
    }
}
