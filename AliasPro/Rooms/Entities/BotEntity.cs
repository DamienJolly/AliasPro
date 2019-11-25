using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Utilities;

namespace AliasPro.Rooms.Entities
{
    internal class BotEntity : BaseEntity
    {
        internal BotEntity(int botId, uint ownerId, string ownerUsername, int id, int x, int y, int rotation, IRoom room, string name, string figure, PlayerGender gender, string motto, int score, int danceId, bool canWalk)
            : base(id, x, y, rotation, room, name, figure, gender, motto, score)
        {
			BotId = botId;
			OwnerId = ownerId;
			OwnerUsername = ownerUsername;
			DanceId = danceId;
			CanWalk = canWalk;
		}

		public int BotId { get; set; }
		public uint OwnerId { get; set; }
		public string OwnerUsername { get; set; }
		public bool CanWalk { get; set; }

		private int ActionTimer = 0;
		private int SpeechTimer = 0;

		public override async void OnEntityJoin()
		{
			SpeechTimer = 20;
			ActionTimer = Randomness.RandomNumber(5, 20);

			await Room.SendAsync(new UserDanceComposer(this));
		}

		public override void OnEntityLeave()
		{

		}

		public override void Cycle()
        {
			if (SpeechTimer <= 0)
			{
				Room.OnChat("testing", 1, this);
				SpeechTimer = 20;
			}
			else
			{
				SpeechTimer--;
			}

			if (CanWalk)
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
			serverPacket.WriteString(OwnerUsername);
			serverPacket.WriteInt(5);
			{
				serverPacket.WriteShort(1); //Copy looks
				serverPacket.WriteShort(2); //Setup speech
				serverPacket.WriteShort(3); //Relax
				serverPacket.WriteShort(4); //Dance
				serverPacket.WriteShort(5); //Change name
			}
		}
	}
}
