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
            Interaction = reader.ReadData<string>("interaction_type");
            CanWalk = reader.ReadData<bool>("can_walk");
            CanStack = reader.ReadData<bool>("can_stack");
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
        public string Interaction { get; }
        public bool CanWalk { get; }
        public bool CanStack { get; }
    }
}
