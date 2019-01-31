using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Landing
{
    using Database;
    using Models;

    internal class LandingDao : BaseDao
    {
        internal async Task<IList<IHallOfFamer>> GetHallOfFamers()
        {
            IList<IHallOfFamer> hallOfFamers = new List<IHallOfFamer>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        hallOfFamers.Add(new HallOfFamer(reader));
                    }
                }, "SELECT `id`, `username`, `figure`, `credits` AS `amount` FROM `players` WHERE `rank` < 5 ORDER BY `amount` DESC LIMIT 10;");
            });

            return hallOfFamers;
        }

        internal async Task<IList<IArticles>> GetNewsArticles()
        {
            IList<IArticles> hallOfFamers = new List<IArticles>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        hallOfFamers.Add(new Articles(reader));
                    }
                }, "SELECT `id`, `title`, `text`, `caption`, `type`, `link`, `image` FROM `landing_articles` ORDER BY `id` DESC LIMIT 10;");
            });

            return hallOfFamers;
        }
    }
}
