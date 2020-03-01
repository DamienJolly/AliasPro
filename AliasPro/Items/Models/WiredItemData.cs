using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Models;

namespace AliasPro.Items.Models
{
    internal class WiredItemData : IWiredItemData
    {
        internal WiredItemData(string[] itemParts)
        {
            ItemId = uint.Parse(itemParts[0]);

            int x = int.Parse(itemParts[1]);
            int y = int.Parse(itemParts[2]);
            double z = double.Parse(itemParts[3]);
            Position = new RoomPosition(x, y, x);

            ExtraData = itemParts[4];
            Rotation = int.Parse(itemParts[5]);
            MovementDirection = int.Parse(itemParts[6]);
        }

        internal WiredItemData(uint itemId, IRoomPosition position, string extraData, int rotation)
        {
            ItemId = itemId;
            Position = position;
            ExtraData = extraData;
            Rotation = rotation;
            MovementDirection = -1;
        }

        public override string ToString()
        {
            return ItemId + ":" + Position.X + ":" + Position.Y + ":" + Position.Z + ":" + ExtraData + ":" + Rotation + ":" + MovementDirection;
        }

        public uint ItemId { get; set; }
        public IRoomPosition Position { get; set; }
        public string ExtraData { get; set; }
        public int Rotation { get; set; }
        public int MovementDirection { get; set; }
    }
}
