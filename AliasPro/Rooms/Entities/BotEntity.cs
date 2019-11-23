using AliasPro.API.Players.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Entities
{
    internal class BotEntity : BaseEntity
    {
        internal BotEntity(int botId, uint ownerId, string ownerUsername, int id, int x, int y, int rotation, IRoom room, string name, string figure, PlayerGender gender, string motto, int score)
            : base(id, x, y, rotation, room, name, figure, gender, motto, score)
        {
			BotId = botId;
			OwnerId = ownerId;
			OwnerUsername = ownerUsername;
		}

		public int BotId { get; set; }
		public uint OwnerId { get; set; }
		public string OwnerUsername { get; set; }

        public override void Cycle()
        {

        }

        public override void Compose(ServerPacket serverPacket)
        {
            serverPacket.WriteInt(Id); //botId?
            serverPacket.WriteString(Name);
            serverPacket.WriteString(Motto);
            serverPacket.WriteString(Figure);
            serverPacket.WriteInt(Id);
            serverPacket.WriteInt(Position.X);
            serverPacket.WriteInt(Position.Y);
            serverPacket.WriteString(Position.Z.ToString());
			serverPacket.WriteInt(BodyRotation);

			serverPacket.WriteInt(4);
			serverPacket.WriteString(Gender == PlayerGender.MALE ? "m" : "f"); // ?
			serverPacket.WriteInt(OwnerId);
			serverPacket.WriteString(OwnerUsername); // Owner name
			serverPacket.WriteInt(5);
			{
				serverPacket.WriteShort(1);//Copy looks
				serverPacket.WriteShort(2);//Setup speech
				serverPacket.WriteShort(3);//Relax
				serverPacket.WriteShort(4);//Dance
				serverPacket.WriteShort(5);//Change name
			}
        }
    }
}
