using System.Threading.Tasks;

namespace AliasPro.Player
{
    using Models;

    internal class PlayerDao
    {
        internal async Task<IPlayer> GetPlayerById(uint id)
        {
            IPlayer player = null;
            //todo: pull from database
            {
                player = new Player();
            }
            return player;
        }

        internal async Task<IPlayer> GetPlayerBySso(string sso)
        {
            IPlayer player = null;
            {
                player = new Player();
            }
            return player;
        }
    }
}
