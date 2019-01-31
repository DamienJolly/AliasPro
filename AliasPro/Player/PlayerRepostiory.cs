using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Player
{
    using Models;

    internal class PlayerRepostiory
    {
        private readonly PlayerDao _playerDao;
        private readonly Dictionary<uint, IPlayer> _players;

        public PlayerRepostiory(PlayerDao playerDao)
        {
            _playerDao = playerDao;

            _players = new Dictionary<uint, IPlayer>();
        }

        internal async Task<IPlayer> GetPlayerById(uint id)
        {
            if (_players.TryGetValue(id, out IPlayer player)) return player;

            player = await _playerDao.GetPlayerById(id);
            _players.Add(player.Id, player);

            return player;
        }

        internal async Task<IPlayer> GetPlayerBySso(string sso) =>
            await _playerDao.GetPlayerBySso(sso);
    }
}
