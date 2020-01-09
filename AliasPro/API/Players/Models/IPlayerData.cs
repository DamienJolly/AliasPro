using AliasPro.API.Groups.Models;
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
        IDictionary<int, IGroup> Groups { get; set; }
        int HomeRoom { get; set; }

        bool IsFavoriteGroup(int groupId);
        bool HasGroup(int groupId);
        bool TryGetGroup(int groupId, out IGroup group);
        bool TryAddGroup(IGroup group);
        void RemoveGroup(int groupId);
	}
}
