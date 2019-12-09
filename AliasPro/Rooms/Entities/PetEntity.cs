using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using AliasPro.Utilities;
using System;

namespace AliasPro.Rooms.Entities
{
    public class PetEntity : BaseEntity
    {
        internal PetEntity(int petId, int type, int race, string colour, int experience, int happyness, int energy, int hunger, int thirst, int respect, int created, uint ownerId, string ownerUsername, int id, int x, int y, int rotation, IRoom room, string name)
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
			Created = created;
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
		public int Created { get; set; }

        private int WalkTimer = 0;
        private int ActionTimer = 0;
        private int DecayTimer = 0;
        private int HappynessTimer = 0;
        private int GestureTime = 0;

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
            if (GestureTime <= 0)
            {
                Actions.RemoveStatus("gst");
            }
            else
            {
                GestureTime--;
            }

            if (Actions.HasStatus("lay"))
            {
                Energy += 5;
                Happyness++;

                if (Energy > 100)
                    Energy = 100;

                if (Happyness > 100)
                    Happyness = 100;

                if (Energy >= 100)
                {
                    if (Room.RoomGrid.TryGetRandomWalkableTile(out IRoomTile tile))
                    {
                        GoalPosition = tile.Position;
                        Actions.RemoveStatus("lay");
                        WalkTimer = Randomness.RandomNumber(5, 20);
                    }
                }
            }
            else
            {
                if (DecayTimer <= 0)
                {
                    if (Hunger < 100)
                        Hunger++;

                    if (Thirst < 100)
                        Thirst++;

                    if (Energy > 0)
                        Energy--;

                    DecayTimer = 10;
                }
                else
                {
                    DecayTimer--;
                }

                if (HappynessTimer <= 0)
                {
                    if (Happyness > 0)
                        Happyness--;

                    HappynessTimer = 30;
                }
                else
                {
                    HappynessTimer--;
                }

                if (!Actions.HasStatus("mv"))
                {
                    if (ActionTimer <= 0)
                    {
                        if (Energy <= 30)
                        {
                            Room.OnChat("I'm tired", 0, this);
                            Actions.AddStatus("gst", "trd");
                            FindNest();
                        }
                        else if (Happyness >= 100)
                        {
                            Room.OnChat("I'm in love", 0, this);
                            Actions.AddStatus("gst", "lov");
                        }
                        else if (Happyness >= 85)
                        {
                            Room.OnChat("I'm happy", 0, this);
                            Actions.AddStatus("gst", "sml");
                        }
                        else if (Happyness <= 15)
                        {
                            Room.OnChat("I'm sad", 0, this);
                            Actions.AddStatus("gst", "sad");
                        }
                        else if (Hunger >= 50)
                        {
                            Room.OnChat("I'm hungry", 0, this);
                            Actions.AddStatus("gst", "hng");
                            FindFood();
                        }
                        else if (Thirst >= 50)
                        {
                            Room.OnChat("I'm thirsty", 0, this);
                            Actions.AddStatus("gst", "thr");
                            FindDrink();
                        }

                        GestureTime = 15;
                        ActionTimer = Randomness.RandomNumber(9, 60);
                        return;
                    }
                    else
                    {
                        ActionTimer--;
                    }

                    if (WalkTimer <= 0)
                    {
                        if (Room.RoomGrid.TryGetRandomWalkableTile(out IRoomTile tile))
                        {
                            GoalPosition = tile.Position;
                            WalkTimer = Randomness.RandomNumber(5, 20);
                            return;
                        }
                    }
                    else
                    {
                        WalkTimer--;
                    }
                }
            }
        }

        public void FindNest()
        {
            //find nest types
            Actions.AddStatus("lay", 0.00 + "");
            IsLaying = true;
        }

        public void FindFood()
        {
            //find food types
        }

        public void FindDrink()
        {
            //find drink types
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

		public int ExperienceLeft =>
			PetLevel >= _experiences.Length + 1 ? Experience : _experiences[PetLevel - 1];


		public int DaysOld =>
			(int)Math.Floor((UnixTimestamp.Now - Created) / (3600 * 24)) + 1;

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
