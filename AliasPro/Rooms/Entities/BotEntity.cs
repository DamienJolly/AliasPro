using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
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

			await Room.SendPacketAsync(new UserDanceComposer(this));
		}

		public override void OnEntityLeave()
		{

		}

		public override void Cycle()
        {
			if (SpeechTimer <= 0)
			{
				//Room.OnChat("testing", 1, this);
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

        public override void Compose(ServerMessage ServerMessage)
        {
            ServerMessage.WriteInt(-BotId); //botId?
            ServerMessage.WriteString(Name);
            ServerMessage.WriteString(Motto);
            ServerMessage.WriteString(Figure);
            ServerMessage.WriteInt(Id);
            ServerMessage.WriteInt(Position.X);
            ServerMessage.WriteInt(Position.Y);
            ServerMessage.WriteDouble(Position.Z);
			ServerMessage.WriteInt(BodyRotation);

			ServerMessage.WriteInt(4);
			ServerMessage.WriteString(Gender == PlayerGender.MALE ? "m" : "f"); // ?
			ServerMessage.WriteInt((int)OwnerId);
			ServerMessage.WriteString(OwnerUsername);
			ServerMessage.WriteInt(4);
			{
				ServerMessage.WriteShort(1); //Copy looks
				//ServerMessage.WriteShort(2); //Setup speech
				ServerMessage.WriteShort(3); //Relax
				ServerMessage.WriteShort(4); //Dance
				ServerMessage.WriteShort(5); //Change name
			}
		}
	}
}
