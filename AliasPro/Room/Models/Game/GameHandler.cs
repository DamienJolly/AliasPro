using AliasPro.Item.Models;
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

        public void StartGame()
        {
            GameStarted = true;
            _room.ItemHandler.TriggerWired(WiredInteraction.GAME_STARTS);
        }

        public void EndGame()
        {
            GameStarted = false;
            _room.ItemHandler.TriggerWired(WiredInteraction.GAME_ENDS);
        }

        private void ResetTeams()
        {
            foreach (GameTeam team in _teams.Values)
                team.Points = 0;
        }

        public void GiveTeamPoints(GameTeamType teamType, int amount = 1)
        {
            _teams[teamType].Points += amount;
            //_room.ItemHandler.TriggerWired(WiredInteraction.SCORE_ACHIEVED);
        }
    }
}
