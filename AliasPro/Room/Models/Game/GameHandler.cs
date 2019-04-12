using AliasPro.Items.Models;
using AliasPro.Room.Models.Entities;
using System.Collections.Generic;

namespace AliasPro.Room.Models.Game
{
    public class GameHandler
    {
        private readonly IRoom _room;
        private readonly IDictionary<GameTeamType, GameTeam> _teams;

        public bool GameStarted = false;
        
        public GameHandler(IRoom room)
        {
            _room = room;

            _teams = new Dictionary<GameTeamType, GameTeam>
            {
                { GameTeamType.YELLOW, new GameTeam() },
                { GameTeamType.BLUE, new GameTeam() },
                { GameTeamType.GREEN, new GameTeam() },
                { GameTeamType.RED, new GameTeam() }
            };
        }

        public void LeaveTeam(BaseEntity entity)
        {
            if (!TryGetTeam(entity.Team, out GameTeam team))
                return;

            entity.Team = GameTeamType.NONE;
            team.LeaveTeam(entity);
        }

        public void JoinTeam(BaseEntity entity, GameTeamType teamType)
        {
            if (!TryGetTeam(teamType, out GameTeam team))
                return;

            entity.Team = teamType;
            team.JoinTeam(entity);
        }

        public void StartGame()
        {
            GameStarted = true;
            _room.ItemHandler.TriggerWired(WiredInteraction.GAME_STARTS);
            _room.ItemHandler.TriggerWired(WiredInteraction.AT_GIVEN_TIME);
        }

        public void EndGame()
        {
            ResetTeams();
            GameStarted = false;
            _room.ItemHandler.TriggerWired(WiredInteraction.GAME_ENDS);
        }
        
        private void ResetTeams()
        {
            foreach (GameTeam team in _teams.Values)
            {
                team.MaxPoints = 0;
                team.Points = 0;
            }
        }

        public void GiveTeamPoints(GameTeamType teamType, int amount)
        {
            if (!GameStarted) return;

            if (!TryGetTeam(teamType, out GameTeam team))
                return;

            team.Points += amount;
            System.Console.WriteLine("points added: " + amount);
            _room.ItemHandler.TriggerWired(WiredInteraction.SCORE_ACHIEVED, team.Points);
        }

        public void GiveTeamPoints(GameTeamType teamType, int amount, int maxAmount)
        {
            if (!GameStarted) return;

            if (!TryGetTeam(teamType, out GameTeam team))
                return;

            if (maxAmount > team.MaxPoints)
            {
                team.MaxPoints++;
                team.Points += amount;
                System.Console.WriteLine("points added: " + amount);
                _room.ItemHandler.TriggerWired(WiredInteraction.SCORE_ACHIEVED, team.Points);
            }
        }

        public bool TryGetTeam(GameTeamType teamType, out GameTeam team) =>
            _teams.TryGetValue(teamType, out team);
    }
}
