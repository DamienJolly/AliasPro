using AliasPro.API.Rooms.Games;
using AliasPro.Rooms.Games.Types;
using System.Collections.Generic;

namespace AliasPro.Rooms.Components
{
    public class GameComponent
    {
        private readonly IDictionary<GameType, BaseGame> _games;

        public GameComponent()
        {
            _games = new Dictionary<GameType, BaseGame>();
        }

        public bool TryGetGame(GameType type, out BaseGame game) =>
            _games.TryGetValue(type, out game);

        public bool TryAddGame(BaseGame game) =>
            _games.TryAdd(game.Type, game);

        public void RemoveGame(GameType type) =>
            _games.Remove(type);

        public void StartGames()
        {
            foreach (BaseGame game in _games.Values)
            {
                game.Initialize();
                game.StartGame();
            }
        }

        public void EndGames()
        {
            foreach (BaseGame game in _games.Values)
            {
                game.EndGame();
            }
        }

        public void PauseGames()
        {
            foreach (BaseGame game in _games.Values)
                game.PuseGame();
        }

        public void UnpauseGames()
        {
            foreach (BaseGame game in _games.Values)
                game.UnpauseGame();
        }

        public ICollection<BaseGame> Games =>
            _games.Values;
    }
}
