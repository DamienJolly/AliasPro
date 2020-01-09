using AliasPro.API.Database;
using AliasPro.API.Groups.Models;
using AliasPro.API.Players.Models;
using AliasPro.Players.Types;
using System.Collections.Generic;
using System.Data.Common;

namespace AliasPro.Players.Models
{
    internal class PlayerData : IPlayerData
    {
        internal PlayerData(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            Credits = reader.ReadData<int>("credits");
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
            Groups = new Dictionary<int, IGroup>();
            HomeRoom = reader.ReadData<int>("home_room");
        }

        internal PlayerData(IPlayerData data)
        {
            Id = data.Id;
            Credits = data.Credits;
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
        }

        public uint Id { get; set; }
        public int Credits { get; set; }
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
        public IDictionary<int, IGroup> Groups { get; set; }
        public int HomeRoom { get; set; }

        public bool IsFavoriteGroup(int groupId) =>
            FavoriteGroup == groupId;
        public bool HasGroup(int groupId) =>
            Groups.ContainsKey(groupId);
        public bool TryGetGroup(int groupId, out IGroup group) =>
            Groups.TryGetValue(groupId, out group);
        public bool TryAddGroup(IGroup group) =>
            Groups.TryAdd(group.Id, group);
        public void RemoveGroup(int groupId) =>
            Groups.Remove(groupId);
    }
}
