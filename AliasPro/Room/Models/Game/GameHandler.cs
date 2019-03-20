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

            _teams = new Dictionary<GameTeamType, GameTeam>();
            _teams.Add(GameTeamType.YELLOW, new GameTeam());
            _teams.Add(GameTeamType.BLUE, new GameTeam());
            _teams.Add(GameTeamType.GREEN, new GameTeam());
            _teams.Add(GameTeamType.RED, new GameTeam());
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
    }
}
