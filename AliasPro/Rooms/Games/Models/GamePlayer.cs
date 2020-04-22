using AliasPro.API.Rooms.Games;
using AliasPro.API.Rooms.Games.Models;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Types;

namespace AliasPro.Rooms.Games.Models
{
    internal class GamePlayer : IGamePlayer
    {
        internal GamePlayer(PlayerEntity entity, IGameTeam team, BaseGame game)
        {
            Entity = entity;
            Team = team;
            Game = game;
        }

        public PlayerEntity Entity { get; set; }
        public IGameTeam Team { get; set; }
        public BaseGame Game { get; set; }
        public int Points { get; set; } = 0;
    }
}
