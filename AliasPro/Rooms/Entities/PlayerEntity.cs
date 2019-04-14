using AliasPro.API.Players.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Entities
{
    internal class PlayerEntity : BaseEntity
    {
        internal PlayerEntity(int id, int x, int y, int rotation, ISession session)
            : base(id, x, y, rotation, session.Player.Username, session.Player.Figure, session.Player.Gender, session.Player.Motto)
        {
            Session = session;
        }

        public ISession Session { get; }
        public IPlayer Player => Session.Player;

        public override async void CycleEntity()
        {
            System.Console.WriteLine("player cycle");
            if (HandItemId != 0)
            {
                HandItemTimer--;
                if (HandItemTimer <= 0)
                {
                    SetHandItem(0);
                    await Session.CurrentRoom.SendAsync(new UserHandItemComposer(Id, HandItemId));
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

        public override void Compose(ServerPacket serverPacket)
        {
            serverPacket.WriteInt(Player.Id);
            serverPacket.WriteString(Name);
            serverPacket.WriteString(Motto);
            serverPacket.WriteString(Figure);
            serverPacket.WriteInt(Id);
            serverPacket.WriteInt(Position.X);
            serverPacket.WriteInt(Position.Y);
            serverPacket.WriteString(Position.Z.ToString());
            serverPacket.WriteInt(0);
            serverPacket.WriteInt(1);
            serverPacket.WriteString(Player.Gender == PlayerGender.MALE ? "m" : "f");

            serverPacket.WriteInt(-1);
            serverPacket.WriteInt(-1);
            serverPacket.WriteInt(0);
            serverPacket.WriteInt(1337); // achievement points
            serverPacket.WriteBoolean(false);
        }
    }
}
