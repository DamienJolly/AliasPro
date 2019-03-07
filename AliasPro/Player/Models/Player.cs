using System.Data.Common;

namespace AliasPro.Player.Models
{
    using Sessions;
    using Database;
    using Currency;
    using Messenger;
    using Badge;

    internal class Player : IPlayer
    {
        internal Player(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            Credits = reader.ReadData<int>("credits");
            Rank = reader.ReadData<int>("rank");
            Username = reader.ReadData<string>("username");
            SsoTicket = reader.ReadData<string>("auth_ticket");
            Figure = reader.ReadData<string>("figure");
            Gender = reader.ReadData<string>("gender");
            Motto = reader.ReadData<string>("motto");
            IsOnline = reader.ReadData<bool>("is_online");
        }
        
        public uint Id { get; set; }
        public int Credits { get; set; }
        public int Rank { get; set; }
        public string Username { get; set; }
        public string SsoTicket { get; set; }
        public string Figure { get; set; }
        public string Gender { get; set; }
        public string Motto { get; set; }
        public bool IsOnline { get; set; }

        public ISession Session { get; set; }

        public IPlayerSettings PlayerSettings { get; set; }
        public IPlayerInventory Inventory { get; set; }
        public PlayerCurrency Currency { get; set; }
        public MessengerHandler Messenger { get; set; }
        public BadgeHandler Badge { get; set; }
    }

    public interface IPlayer
    {
        uint Id { get; set; }
        int Credits { get; set; }
        int Rank { get; set; }
        string Username { get; set; }
        string SsoTicket { get; set; }
        string Figure { get; set; }
        string Gender { get; set; }
        string Motto { get; set; }
        bool IsOnline { get; set; }

        ISession Session { get; set; }

        IPlayerSettings PlayerSettings { get; set; }
        IPlayerInventory Inventory { get; set; }
        PlayerCurrency Currency { get; set; }
        MessengerHandler Messenger { get; set; }
        BadgeHandler Badge { get; set; }
    }
}
