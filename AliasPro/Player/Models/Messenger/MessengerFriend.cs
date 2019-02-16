using System.Data.Common;

namespace AliasPro.Player.Models.Messenger
{
    using Network.Protocol;
    using Database;

    internal class MessengerFriend : IMessengerFriend
    {
        public MessengerFriend(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            Username = reader.ReadData<string>("username");
            Figure = reader.ReadData<string>("figure");
            Gender = reader.ReadData<string>("gender");
            Motto = reader.ReadData<string>("motto");
            IsOnline = reader.ReadData<bool>("is_online");
            InRoom = false;
            Relation = reader.ReadData<int>("relation");
        }

        public MessengerFriend(IPlayer player)
        {
            Id = player.Id;
            Username = player.Username;
            Figure = player.Figure;
            Gender = player.Gender;
            Motto = player.Motto;
            IsOnline = player.IsOnline;
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

        public uint Id { get;}
        public string Username { get; set; }
        public string Figure { get; set; }
        public string Gender { get; set; }
        public string Motto { get; set; }
        public bool InRoom { get; set; }
        public bool IsOnline { get; set; }
        public int Relation { get; set; }
    }

    public interface IMessengerFriend
    {
        void Compose(ServerPacket message);

        uint Id { get; }
        string Username { get; set; }
        string Figure { get; set; }
        string Gender { get; set; }
        string Motto { get; set; }
        bool InRoom { get; set; }
        bool IsOnline { get; set; }
        int Relation { get; set; }
    }
}
