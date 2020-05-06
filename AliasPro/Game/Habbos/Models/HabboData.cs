using System;

namespace AliasPro.Game.Habbos.Models
{
    internal class HabboData
    {
        public HabboData(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            Rank = reader.ReadData<int>("rank");
            Username = reader.ReadData<string>("username");
            Figure = reader.ReadData<string>("figure");

            switch (reader.ReadData<string>("gender").ToLower())
            {
                case "m": default: Gender = PlayerGender.MALE; break;
                case "f": Gender = PlayerGender.FEMALE; break;
            }

            Motto = reader.ReadData<string>("motto");
            Online = reader.ReadData<bool>("is_online");
            Score = reader.ReadData<int>("score");
            CreatedAt = reader.ReadData<int>("created_at");
            LastOnline = reader.ReadData<int>("last_online");
            FavoriteGroup = reader.ReadData<int>("group_id");
            Groups = new List<int>();
            HomeRoom = reader.ReadData<int>("home_room");
            VipRank = reader.ReadData<int>("vip_rank");
            Respects = reader.ReadData<int>("respects");
            RespectsGiven = reader.ReadData<int>("respects_given");
            RespectsRecieved = reader.ReadData<int>("respects_recieved");
            LoginStreak = reader.ReadData<int>("login_streak");
        }

        public HabboData(HabboData data)
        {
            Id = data.Id;
            Rank = data.Rank;
            Username = data.Username;
            Figure = data.Figure;
            Gender = data.Gender;
            Motto = data.Motto;
            Online = data.Online;
            Score = data.Score;
            CreatedAt = data.CreatedAt;
            LastOnline = data.LastOnline;
            FavoriteGroup = data.FavoriteGroup;
            Groups = data.Groups;
            HomeRoom = data.HomeRoom;
            VipRank = data.VipRank;
            Respects = data.Respects;
            RespectsGiven = data.RespectsGiven;
            RespectsRecieved = data.RespectsRecieved;
            LoginStreak = data.LoginStreak;
        }

        public uint Id { get; set; }
        public int Rank { get; set; }
        public string Username { get; set; }
        public string Figure { get; set; }
        public PlayerGender Gender { get; set; }
        public string Motto { get; set; }
        public bool Online { get; set; }
        public int Score { get; set; }
        public int CreatedAt { get; set; }
        public int LastOnline { get; set; }
        public int FavoriteGroup { get; set; }
        public IList<int> Groups { get; set; }
        public int HomeRoom { get; set; }
        public int VipRank { get; set; }
        public int Respects { get; set; }
        public int RespectsGiven { get; set; }
        public int RespectsRecieved { get; set; }
        public int LoginStreak { get; set; }

        public bool IsFavoriteGroup(int groupId) =>
            FavoriteGroup == groupId;
        public bool HasGroup(int groupId) =>
            Groups.Contains(groupId);
        public void AddGroup(int groupId) =>
            Groups.Add(groupId);
        public void RemoveGroup(int groupId) =>
            Groups.Remove(groupId);
    }
}
