using AliasPro.Room.Models.Entities;
using System.Collections.Generic;

namespace AliasPro.Room.Models.Game
{
    public class GameTeam
    {
        public int Points;
        public int MaxPoints;
        public IList<BaseEntity> Members;

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