using AliasPro.API.Rooms.Games;
using AliasPro.Rooms.Games.Types;
using System.Collections.Generic;

namespace AliasPro.Rooms.Components
{
    public class GameComponentNew
    {
        private readonly IDictionary<GameType, BaseGame> _games;

        public GameComponentNew()
        {
            _games = new Dictionary<GameType, BaseGame>();
        }

        public bool TryGetGame(GameType type, out BaseGame game) =>
            _games.TryGetValue(type, out game);

        public bool TryAddGame(BaseGame game) =>
            _games.TryAdd(game.Type, game);

        public void RemoveGame(GameType type) =>
            _games.Remove(type);

        public ICollection<BaseGame> Games =>
            _games.Values;
    }
}
