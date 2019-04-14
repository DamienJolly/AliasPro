using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using System.Collections.Generic;

namespace AliasPro.Rooms.Models
{
    internal class GameTeam : IGameTeam
    {
        public int Points { get; set; }
        public int MaxPoints { get; set; }
        public IList<BaseEntity> Members { get; set; }

        internal GameTeam()
        {
            Points = 0;
            MaxPoints = 0;
            Members = new List<BaseEntity>();
        }

        public void JoinTeam(BaseEntity entity)
        {
            if (!Members.Contains(entity))
                Members.Add(entity);
        }

        public void LeaveTeam(BaseEntity entity)
        {
            if (Members.Contains(entity))
                Members.Remove(entity);
        }
    }
}
