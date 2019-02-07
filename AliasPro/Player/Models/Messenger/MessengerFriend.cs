using System.Data.Common;

namespace AliasPro.Player.Models.Messenger
{
    using Network.Protocol;
    using Database;

    internal class MessengerFriend : IMessengerFriend
    {
        //todo: use player class
        public MessengerFriend(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            Username = reader.ReadData<string>("username");
            Figure = reader.ReadData<string>("figure");
            Motto = reader.ReadData<string>("motto");
            IsOnline = true;
            InRoom = false;
            Relation = reader.ReadData<int>("relation");
        }

        public MessengerFriend(IPlayer player)
        {
            Id = player.Id;
            Username = player.Username;
            Figure = player.Figure;
            Motto = player.Motto;
            IsOnline = true;
            InRoom = false;
            Relation = 0;
        }

        public void Compose(ServerPacket message)
        {
            message.WriteInt(Id);
            message.WriteString(Username);
            message.WriteInt(1); //Gender???
            message.WriteBoolean(IsOnline); //Online
            message.WriteBoolean(InRoom);
            message.WriteString(Figure);
            message.WriteInt(0); //category id
            message.WriteString(Motto);
            message.WriteString("");
            message.WriteString("");
            message.WriteBoolean(true);
            message.WriteBoolean(false);
            message.WriteBoolean(false);
            message.WriteShort((short)Relation);
        }

        public uint Id { get; }
        public string Username { get; }
        public string Figure { get; }
        public string Motto { get; }
        public bool InRoom { get; }
        public bool IsOnline { get; }
        public int Relation { get; }
    }

    public interface IMessengerFriend
    {
        void Compose(ServerPacket message);

        uint Id { get; }
        string Username { get; }
        string Figure { get; }
        string Motto { get; }
        bool InRoom { get; }
        bool IsOnline { get; }
        int Relation { get; }
    }
}
