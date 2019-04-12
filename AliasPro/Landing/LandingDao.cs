using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Landing.Models;
using AliasPro.Landing.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Landing
{
    internal class LandingDao : BaseDao
    {
        public LandingDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }

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

        internal async Task<IList<IArticle>> GetNewsArticles()
        {
            IList<IArticle> hallOfFamers = new List<IArticle>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        hallOfFamers.Add(new Article(reader));
                    }
                }, "SELECT `id`, `title`, `text`, `caption`, `type`, `link`, `image` FROM `landing_articles` ORDER BY `id` DESC LIMIT 10;");
            });

            return hallOfFamers;
        }
    }
}
