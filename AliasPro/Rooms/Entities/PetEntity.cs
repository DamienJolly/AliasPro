using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using AliasPro.Utilities;

namespace AliasPro.Rooms.Entities
{
    public class PetEntity : BaseEntity
    {
        internal PetEntity(int petId, int type, int race, string colour, int experience, int happyness, int energy, int hunger, int thirst, int respect, uint ownerId, string ownerUsername, int id, int x, int y, int rotation, IRoom room, string name)
            : base(id, x, y, rotation, room, name, "", PlayerGender.MALE, "", 0)
        {
			PetId = petId;
			OwnerId = ownerId;
			OwnerUsername = ownerUsername;
			Type = type;
			Race = race;
			Colour = colour;
			Experience = experience;
			Happyness = happyness;
			Energy = energy;
			Hunger = hunger;
			Thirst = thirst;
			Respect = respect;
		}

		public int PetId { get; set; }
		public uint OwnerId { get; set; }
		public string OwnerUsername { get; set; }
		public int Type { get; set; }
		public int Race { get; set; }
		public string Colour { get; set; }
		public int Experience { get; set; }
		public int Happyness { get; set; }
		public int Energy { get; set; }
		public int Hunger { get; set; }
		public int Thirst { get; set; }
		public int Respect { get; set; }

		private int ActionTimer = 0;

		private readonly int[] _experiences = new int[]
		{
			100, 200, 400, 600, 900, 1300, 1800, 2400, 3200, 4300, 5700, 7600, 10100, 13300, 17500, 23000, 30200, 39600, 51900
		};

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
            serverPacket.WriteString(string.Empty);
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
			serverPacket.WriteInt(PetLevel); // level
			serverPacket.WriteString(string.Empty); // ??
		}

		public int PetLevel
		{
			get
			{
				int index = _experiences.Length;

				for (int i = 0; i < _experiences.Length; i++)
				{
					if (_experiences[i] > Experience)
					{
						index = i;
						break;
					}
				}

				return index + 1;
			}
		}

		// Deprecated
		/*public int GetLevelFromXp(int xp)
		{
			int level = 0;

			if (xp >= 0)
			{
				for (int i = 0; i <= level - 2; i++)
				{
					xp += 100 * (int)Math.Round(Math.Pow(1.30637788, i), 0);
				}
			}

			return xp;
		}*/
	}
}
