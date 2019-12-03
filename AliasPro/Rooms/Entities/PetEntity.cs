using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;

namespace AliasPro.Rooms.Entities
{
    internal class PetEntity : BaseEntity
    {
        internal PetEntity(int petId, int type, uint ownerId, string ownerUsername, int id, int x, int y, int rotation, IRoom room, string name, string figure, PlayerGender gender, string motto, int score)
            : base(id, x, y, rotation, room, name, figure, gender, motto, score)
        {
			PetId = petId;
			Type = type;
			OwnerId = ownerId;
			OwnerUsername = ownerUsername;
		}

		public int PetId { get; set; }
		public uint OwnerId { get; set; }
		public string OwnerUsername { get; set; }
		public int Type { get; set; }

		public override void OnEntityJoin()
		{

		}

		public override void OnEntityLeave()
		{

		}

		public override void Cycle()
        {

		}

        public override void Compose(ServerPacket serverPacket)
        {
            serverPacket.WriteInt(PetId); //petId?
            serverPacket.WriteString(Name);
            serverPacket.WriteString(Motto);
            serverPacket.WriteString(Figure);
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
