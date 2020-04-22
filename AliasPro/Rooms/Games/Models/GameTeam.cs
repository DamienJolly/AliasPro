using AliasPro.API.Rooms.Games.Models;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Types;
using System.Collections.Generic;

namespace AliasPro.Rooms.Games.Models
{
    internal class GameTeam : IGameTeam
    {
        public int TeamPoints { get; set; }
        public GameTeamType Type { get; set; }
        private IDictionary<int, IGamePlayer> _members;

        internal GameTeam(GameTeamType type)
        {
            TeamPoints = 0;
            Type = type;

            _members = new Dictionary<int, IGamePlayer>();
        }

        public bool TryGetPlayer(int playerId, out IGamePlayer player) =>
             _members.TryGetValue(playerId, out player);

        public bool TryJoinTeam(IGamePlayer player) => 
            _members.TryAdd((int)player.Entity.Player.Id, player);

        public void LeaveTeam(PlayerEntity entity) =>
            _members.Remove((int)entity.Player.Id);

        public void ResetScores()
        {
            TeamPoints = 0;
            foreach (IGamePlayer member in _members.Values)
                member.Points = 0;
        }

        public int TotalPoints
        {
            get
            {
                int totalPoints = TeamPoints;
                foreach (IGamePlayer member in _members.Values)
                    totalPoints += member.Points;

                return totalPoints;
            }
        }

        public ICollection<IGamePlayer> Members =>
            _members.Values;
    }
}
