using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Moderation.Models;
using AliasPro.Moderation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Moderation
{
    internal class ModerationDao : BaseDao
    {
        public ModerationDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }

        public async Task<IDictionary<int, IModerationTicket>> ReadTickets()
        {
            IDictionary<int, IModerationTicket> tickets = new Dictionary<int, IModerationTicket>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IModerationTicket ticket = new ModerationTicket(reader);
                        tickets.TryAdd(ticket.Id, ticket);
                    }
                }, "SELECT * FROM `modetation_tickets` WHERE `state` != 0;");
            });
            return tickets;
        }
    }
}
