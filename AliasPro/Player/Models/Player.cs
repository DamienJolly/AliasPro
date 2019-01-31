namespace AliasPro.Player.Models
{
    internal class Player : IPlayer
    {
        internal Player()
        {
            Id = 1;
            Credits = 1337;
            Rank = 1;
            Username = "Damien";
            SsoTicket = "";
            Figure = "";
            Gender = "M";
            Motto = "Hello World!";
        }

        public uint Id { get; set; }
        public int Credits { get; set; }
        public int Rank { get; set; }
        public string Username { get; set; }
        public string SsoTicket { get; set; }
        public string Figure { get; set; }
        public string Gender { get; set; }
        public string Motto { get; set; }
    }
}
