using System.Collections.Generic;

namespace AliasPro.Room.Models.Entities
{
    using Network.Protocol;
    using Gamemap;

    public abstract class BaseEntity
    {
        protected BaseEntity(int id, int x, int y, int rotation, string name, string figure)
        {
            Id = id;
            BodyRotation = rotation;
            Position = new Position(x, y, 0);
            NextPosition = new Position(x, y, 0);
            Name = name;
            Figure = figure;

            ActiveStatuses = new Dictionary<string, string>();
        }
        
        public int Id { get; set; }
        public int BodyRotation { get; set; }
        public Position Position { get; set; }
        public Position NextPosition { get; set; }
        public IList<Position> PathToWalk { get; set; }
        public string Name { get; set; }
        public string Figure { get; set; }

        public IDictionary<string, string> ActiveStatuses { get; set; }
        
        public abstract void Compose(ServerPacket serverPacket);
    }
}
