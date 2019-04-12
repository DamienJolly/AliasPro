using AliasPro.API.Items.Models;
using AliasPro.Room.Gamemap;

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
            Position = new Position(x, y, x);

            Mode = int.Parse(itemParts[4]);
            Rotation = int.Parse(itemParts[5]);
            MovementDirection = int.Parse(itemParts[6]);
        }

        internal WiredItemData(uint itemId, Position position, int mode, int rotation)
        {
            ItemId = itemId;
            Position = position;
            Mode = mode;
            Rotation = rotation;
            MovementDirection = -1;
        }

        public override string ToString()
        {
            return ItemId + ":" + Position.X + ":" + Position.Y + ":" + Position.Z + ":" + Mode + ":" + Rotation + ":" + MovementDirection;
        }

        public uint ItemId { get; set; }
        public Position Position { get; set; }
        public int Mode { get; set; }
        public int Rotation { get; set; }
        public int MovementDirection { get; set; }
    }
}
