using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using AliasPro.Utilities;

namespace AliasPro.Rooms.Entities
{
    public class PetEntity : BaseEntity
    {
        internal PetEntity(int petId, int type, int race, string colour, uint ownerId, string ownerUsername, int id, int x, int y, int rotation, IRoom room, string name, PlayerGender gender, string motto)
            : base(id, x, y, rotation, room, name, "", gender, motto, 0)
        {
			PetId = petId;
			OwnerId = ownerId;
			OwnerUsername = ownerUsername;
			Type = type;
			Race = race;
			Colour = colour;
		}

		public int PetId { get; set; }
		public uint OwnerId { get; set; }
		public string OwnerUsername { get; set; }
		public int Type { get; set; }
		public int Race { get; set; }
		public string Colour { get; set; }

		private int ActionTimer = 0;

		public override void OnEntityJoin()
		{

		}

		public override void OnEntityLeave()
		{

		}

		public override void Cycle()
        {
			if (ActionTimer <= 0)
			{
				if (Room.RoomGrid.TryGetRandomWalkableTile(out IRoomTile tile))
					GoalPosition = tile.Position;

				ActionTimer = Randomness.RandomNumber(5, 20);
			}
			else
			{
				ActionTimer--;
			}
		}

        public override void Compose(ServerPacket serverPacket)
        {
			serverPacket.WriteInt(Id); //petId?
            serverPacket.WriteString(Name);
            serverPacket.WriteString(Motto);
            serverPacket.WriteString(Type + " " + Race + " " + Colour);
			serverPacket.WriteInt(Id);
            serverPacket.WriteInt(Position.X);
            serverPacket.WriteInt(Position.Y);
            serverPacket.WriteString(Position.Z.ToString());
			serverPacket.WriteInt(BodyRotation);

			serverPacket.WriteInt(2);
			serverPacket.WriteInt(Type);
			serverPacket.WriteInt(OwnerId);
			serverPacket.WriteString(OwnerUsername);
			serverPacket.WriteInt(1); // rarity
			serverPacket.WriteBoolean(false); // sadle
			serverPacket.WriteBoolean(false); // riding horse
			serverPacket.WriteBoolean(false); // can breed
			serverPacket.WriteBoolean(false); // fully grown
			serverPacket.WriteBoolean(false); // can revive
			serverPacket.WriteBoolean(false); // public breed?
			serverPacket.WriteInt(0); // level
			serverPacket.WriteString(""); // ??
		}
	}
}
