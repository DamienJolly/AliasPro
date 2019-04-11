﻿namespace AliasPro.Room.Models.Entities
{
    using Network.Protocol;
    using Sessions;
    using AliasPro.API.Player.Models;
    using AliasPro.Player.Models;

    internal class UserEntity : BaseEntity
    {
        internal UserEntity(int id, int x, int y, int rotation, ISession session)
            : base(id, x, y, rotation, session.Player.Username, session.Player.Figure, session.Player.Gender, session.Player.Motto)
        {
            Session = session;
        }

        public ISession Session { get; }
        public IPlayer Player => Session.Player;

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
