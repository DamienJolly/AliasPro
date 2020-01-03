using AliasPro.API.Database;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Players.Models;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using System.Data.Common;

namespace AliasPro.Messenger.Models
{
    internal class MessengerFriend : IMessengerFriend
    {
        public MessengerFriend(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            Username = reader.ReadData<string>("username");
            Figure = reader.ReadData<string>("figure");

            switch (reader.ReadData<string>("gender").ToLower())
            {
                case "m": default: Gender = PlayerGender.MALE; break;
                case "f": Gender = PlayerGender.FEMALE; break;
            }

            Motto = reader.ReadData<string>("motto");
            IsOnline = reader.ReadData<bool>("is_online");
            InRoom = false;
            Relation = 0;
        }

        public MessengerFriend(IPlayer player)
        {
            Id = player.Id;
            Username = player.Username;
            Figure = player.Figure;
            Gender = player.Gender;
            Motto = player.Motto;
            IsOnline = player.Online;
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
        public string Username { get; set; }
        public string Figure { get; set; }
        public PlayerGender Gender { get; set; }
        public string Motto { get; set; }
        public bool InRoom { get; set; }
        public bool IsOnline { get; set; }
        public int Relation { get; set; }
    }
}