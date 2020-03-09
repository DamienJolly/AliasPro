using AliasPro.Players.Types;
using System.Collections.Generic;

namespace AliasPro.API.Players.Models
{
    public interface IPlayerData
    {
        uint Id { get; set; }
        int Credits { get; set; }
        int Rank { get; set; }
        string Username { get; set; }
        string Figure { get; set; }
        PlayerGender Gender { get; set; }
        string Motto { get; set; }
        bool Online { get; set; }
        int Score { get; set; }
        int CreatedAt { get; set; }
        int LastOnline { get; set; }
        int FavoriteGroup { get; set; }
        IList<int> Groups { get; set; }
        int HomeRoom { get; set; }
        int VipRank { get; set; }

        bool IsFavoriteGroup(int groupId);
        bool HasGroup(int groupId);
        void AddGroup(int groupId);
        void RemoveGroup(int groupId);
	}
}
