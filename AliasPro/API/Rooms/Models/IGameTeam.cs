using AliasPro.API.Rooms.Entities;
using System.Collections.Generic;

namespace AliasPro.API.Rooms.Models
{
    internal interface IGameTeam
    {
        int Points { get; set; }
        int MaxPoints { get; set; }
        IList<BaseEntity> Members { get; set; }

        void JoinTeam(BaseEntity entity);
        void LeaveTeam(BaseEntity entity);
    }
}
