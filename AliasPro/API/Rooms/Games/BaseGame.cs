using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Games.Types;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Types;
using System.Collections.Generic;

namespace AliasPro.API.Rooms.Games
{
	public abstract class BaseGame
	{
		private readonly IRoom _room;
		private readonly IDictionary<GameTeamType, IGameTeam> _teams;

		public bool GameStarted = false;
        public readonly IRoom Room = null;

        public abstract GameType Type { get; }

        public BaseGame(IRoom room)
		{
            Room = room;

			_teams = new Dictionary<GameTeamType, IGameTeam>
			{
				{ GameTeamType.YELLOW, new GameTeam() },
				{ GameTeamType.BLUE, new GameTeam() },
				{ GameTeamType.GREEN, new GameTeam() },
				{ GameTeamType.RED, new GameTeam() }
			};
		}

		public virtual void LeaveTeam(BaseEntity entity)
		{
			if (!_teams.TryGetValue(entity.Team, out IGameTeam team))
				return;

			entity.Team = GameTeamType.NONE;
			team.LeaveTeam(entity);
		}

		public virtual void JoinTeam(BaseEntity entity, GameTeamType teamType)
		{
			if (!_teams.TryGetValue(teamType, out IGameTeam team))
				return;

			entity.Team = teamType;
			team.JoinTeam(entity);
		}

        public virtual void StartGame()
        {
            GameStarted = true;
            _room.Items.TriggerWired(WiredInteractionType.GAME_STARTS);
            _room.Items.TriggerWired(WiredInteractionType.AT_GIVEN_TIME);
        }

        public virtual void EndGame()
        {
            ResetTeams();
            GameStarted = false;
            _room.Items.TriggerWired(WiredInteractionType.GAME_ENDS);
        }

        private void ResetTeams()
        {
            foreach (GameTeam team in _teams.Values)
            {
                team.MaxPoints = 0;
                team.Points = 0;
            }
        }

        public virtual void GivePlayerPoints(BaseEntity entity, int amount)
        {
            if (!GameStarted) return;

            if (!_teams.TryGetValue(entity.Team, out IGameTeam team))
                return;

            team.Points += amount;
            _room.Items.TriggerWired(WiredInteractionType.SCORE_ACHIEVED, team.Points);
        }

        public virtual void GiveTeamPoints(GameTeamType teamType, int amount)
        {
            if (!GameStarted) return;

            if (!_teams.TryGetValue(teamType, out IGameTeam team))
                return;

            team.Points += amount;
            _room.Items.TriggerWired(WiredInteractionType.SCORE_ACHIEVED, team.Points);
        }

        public virtual void GiveTeamPoints(GameTeamType teamType, int amount, int maxAmount)
        {
            if (!GameStarted) return;

            if (!_teams.TryGetValue(teamType, out IGameTeam team))
                return;

            if (maxAmount > team.MaxPoints)
            {
                team.MaxPoints++;
                team.Points += amount;
                _room.Items.TriggerWired(WiredInteractionType.SCORE_ACHIEVED, team.Points);
            }
        }
    }
}
