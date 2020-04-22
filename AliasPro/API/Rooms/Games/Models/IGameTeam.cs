using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Types;
using System.Collections.Generic;

namespace AliasPro.API.Rooms.Games.Models
{
    public interface IGameTeam
    {
        int TeamPoints { get; set; }
        int TotalPoints { get; }
        ICollection<IGamePlayer> Members { get; }
        GameTeamType Type { get; set; }

		void LeaveTeam(PlayerEntity entity);
        void ResetScores();
        bool TryGetPlayer(int playerId, out IGamePlayer player);
        bool TryJoinTeam(IGamePlayer player);
    }
}
