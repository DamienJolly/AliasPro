using AliasPro.API.Rooms.Games.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Games.Models;
using AliasPro.Rooms.Games.Types;
using AliasPro.Rooms.Types;
using System.Collections.Generic;

namespace AliasPro.API.Rooms.Games
{
	public abstract class BaseGame
	{
		private readonly IRoom _room;
		private readonly IDictionary<GameTeamType, IGameTeam> _teams;

        public int TimesGivenScore = 0;
		public GameState State = GameState.IDLE;
        public readonly IRoom Room = null;

        public abstract GameType Type { get; }

        public BaseGame(IRoom room)
		{
            Room = room;

			_teams = new Dictionary<GameTeamType, IGameTeam>
			{
				{ GameTeamType.YELLOW, new GameTeam(GameTeamType.YELLOW) },
				{ GameTeamType.BLUE, new GameTeam(GameTeamType.BLUE) },
				{ GameTeamType.GREEN, new GameTeam(GameTeamType.GREEN) },
				{ GameTeamType.RED, new GameTeam(GameTeamType.RED) }
			};
		}

        public abstract void Initialize();

        public bool TryGetTeam(GameTeamType teamType, out IGameTeam team) =>
            _teams.TryGetValue(teamType, out team);

        public virtual void LeaveTeam(PlayerEntity entity)
		{
            if (entity.GamePlayer == null)
                return;

            entity.GamePlayer.Team.LeaveTeam(entity);
            entity.GamePlayer = null;
		}

		public virtual void JoinTeam(PlayerEntity entity, GameTeamType teamType)
		{
            if (entity.GamePlayer != null)
                LeaveTeam(entity);

            if (!_teams.TryGetValue(teamType, out IGameTeam team))
				return;

            IGamePlayer gamePlayer = new GamePlayer(entity, team, this);
            if (!team.TryJoinTeam(gamePlayer))
                return;

            entity.GamePlayer = gamePlayer;
		}

        public virtual void StartGame()
        {
            State = GameState.RUNNING;
        }

        public virtual void EndGame()
        {
            State = GameState.IDLE;
        }

        public virtual void PuseGame()
        {
            State = GameState.IDLE;
        }

        public virtual void UnpauseGame()
        {
            State = GameState.RUNNING;
        }

        public ICollection<IGameTeam> Teams =>
            _teams.Values;
    }
}
