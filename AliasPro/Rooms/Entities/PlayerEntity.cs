using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Types;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Entities
{
    public class PlayerEntity : BaseEntity
    {
        internal PlayerEntity(int id, int x, int y, int rotation, ISession session)
            : base(id, x, y, rotation, session.CurrentRoom, session.Player.Username, session.Player.Figure, session.Player.Gender, session.Player.Motto, session.Player.Score)
        {
            Session = session;
        }

        public ISession Session { get; }
        public IPlayer Player => Session.Player;

		public override void OnEntityJoin()
		{

		}

		public override void OnEntityLeave()
		{

		}

		public override async void Cycle()
        {
            if (HandItemId != 0)
            {
                HandItemTimer--;
                if (HandItemTimer <= 0)
                {
                    SetHandItem(0);
                    await Session.CurrentRoom.SendPacketAsync(new UserHandItemComposer(Id, HandItemId));
                }
            }

            if (SignTimer > 0)
            {
                SignTimer--;
                if (SignTimer <= 0)
                {
                    Actions.RemoveStatus("sign");
                }
            }

            IdleTimer++;
            if (IdleTimer >= 600 && !IsIdle)
            {
                IdleTimer = 0;
                IsIdle = true;
                await Session.CurrentRoom.SendPacketAsync(new UserSleepComposer(this));
            }

            if (IdleTimer >= 1800 && IsIdle)
            {
                //todo: kickuser
            }
        }

        public override async void Compose(ServerMessage ServerMessage)
        {
            ServerMessage.WriteInt((int)Player.Id);
            ServerMessage.WriteString(Name);
            ServerMessage.WriteString(Motto);
            ServerMessage.WriteString(Figure);
            ServerMessage.WriteInt(Id);
            ServerMessage.WriteInt(Position.X);
            ServerMessage.WriteInt(Position.Y);
            ServerMessage.WriteDouble(Position.Z);
            ServerMessage.WriteInt(BodyRotation);

            ServerMessage.WriteInt(1);
            ServerMessage.WriteString(Player.Gender == PlayerGender.MALE ? "m" : "f");

            IGroup group = 
                await Program.GetService<IGroupController>().ReadGroupData(Player.FavoriteGroup);
            if (group != null)
            {
                ServerMessage.WriteInt(group.Id);
                ServerMessage.WriteInt(group.Id);
                ServerMessage.WriteString(group.Name);
            }
            else
            {
                ServerMessage.WriteInt(-1);
                ServerMessage.WriteInt(-1);
                ServerMessage.WriteString(string.Empty);
            }

            ServerMessage.WriteString(string.Empty); // dunno?
            ServerMessage.WriteInt(Score);
            ServerMessage.WriteBoolean(true);
        }
    }
}
