using System.Threading.Tasks;

namespace AliasPro.Player
{
    using Database;
    using Models;

    internal class PlayerDao : BaseDao
    {
        internal async Task<IPlayer> GetPlayerById(uint id)
        {
            IPlayer player = null;
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        player = new Player(reader);
                    }
                }, "SELECT `id`, `credits`, `rank`, `username`, `auth_ticket`, `figure`, `gender`, `motto` FROM `players` WHERE `id` = @0 LIMIT 1;", id);
            });
            return player;
        }

        internal async Task<IPlayer> GetPlayerBySso(string sso)
        {
            IPlayer player = null;
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        player = new Player(reader);
                    }
                }, "SELECT `id`, `credits`, `rank`, `username`, `auth_ticket`, `figure`, `gender`, `motto` FROM `players` WHERE `auth_ticket` = @0 LIMIT 1;", sso);
            });
            return player;
        }
    }
}
