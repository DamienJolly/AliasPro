﻿using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Moderation.Models;
using AliasPro.Moderation.Models;
using AliasPro.Utilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Moderation
{
    internal class ModerationDao : BaseDao
    {
        public ModerationDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
			: base(logger, configurationController)
		{

		}

		public async Task<IList<IModerationPreset>> ReadPresets()
		{
			IList<IModerationPreset> presets = new List<IModerationPreset>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						presets.Add(new ModerationPreset(reader));
					}
				}, "SELECT `type`, `preset` FROM `moderation_presets`;");
			});
			return presets;
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
                }, "SELECT * FROM `moderation_tickets` WHERE `state` != 0;");
            });
            return tickets;
        }

        public async Task UpdateTicket(IModerationTicket ticket)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `modetation_tickets` SET `state` = @1, `mod_id` = @2 WHERE `id` = @0;",
                    ticket.Id, (int)ticket.State, ticket.ModId);
            });
        }
	}
}
