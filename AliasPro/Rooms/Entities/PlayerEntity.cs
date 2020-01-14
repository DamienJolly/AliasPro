using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Protocol;
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
                    await Session.CurrentRoom.SendAsync(new UserHandItemComposer(Id, HandItemId));
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
                await Session.CurrentRoom.SendAsync(new UserSleepComposer(this));
            }

            if (IdleTimer >= 1800 && IsIdle)
            {
                //todo: kickuser
            }
        }

        public override async void Compose(ServerPacket serverPacket)
        {
            serverPacket.WriteInt(Player.Id);
            serverPacket.WriteString(Name);
            serverPacket.WriteString(Motto);
            serverPacket.WriteString(Figure);
            serverPacket.WriteInt(Id);
            serverPacket.WriteInt(Position.X);
            serverPacket.WriteInt(Position.Y);
            serverPacket.WriteString(Position.Z.ToString());
            serverPacket.WriteInt(BodyRotation);

            serverPacket.WriteInt(1);
            serverPacket.WriteString(Player.Gender == PlayerGender.MALE ? "m" : "f");

            IGroup group = 
                await Program.GetService<IGroupController>().ReadGroupData(Player.FavoriteGroup);
            if (group != null)
            {
                serverPacket.WriteInt(group.Id);
                serverPacket.WriteInt(group.Id);
                serverPacket.WriteString(group.Name);
            }
            else
            {
                serverPacket.WriteInt(-1);
                serverPacket.WriteInt(-1);
                serverPacket.WriteString(string.Empty);
            }

            serverPacket.WriteString(string.Empty); // dunno?
            serverPacket.WriteInt(Score);
            serverPacket.WriteBoolean(true);
        }
    }
}
