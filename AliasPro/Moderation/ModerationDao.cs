using AliasPro.API.Database;
using AliasPro.API.Moderation.Models;
using AliasPro.Moderation.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Moderation
{
    internal class ModerationDao : BaseDao
    {
        public ModerationDao(ILogger<BaseDao> logger)
			: base(logger)
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

        public async Task<IDictionary<int, IModerationCategory>> ReadCategories()
        {
            IDictionary<int, IModerationCategory> categories = new Dictionary<int, IModerationCategory>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        int categoryId = reader.ReadData<int>("id");
                        IModerationCategory category = new ModerationCategory(
                            reader, 
                            await ReadTopics(categoryId));
                        categories.TryAdd(category.Id, category);
                    }
                }, "SELECT * FROM `moderation_categories`");
            });
            return categories;
        }

        private async Task<IDictionary<int, IModerationTopic>> ReadTopics(int categoryId)
        {
            IDictionary<int, IModerationTopic> topics = new Dictionary<int, IModerationTopic>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IModerationTopic topic = new ModerationTopic(reader);
                        topics.TryAdd(topic.Id, topic);
                    }
                }, "SELECT * FROM `moderation_topics` WHERE `category_id` = @0 AND `enabled` = '1';",
                    categoryId);
            });
            return topics;
        }

        public async Task<int> AddTicket(IModerationTicket ticket)
        {
            int ticketId = -1;
            await CreateTransaction(async transaction =>
            {
                ticketId = await Insert(transaction, "INSERT INTO `moderation_tickets` (`state`, `type`, `category_id`, `timestamp`, `score`, `sender_id`, `reported_id`, `caption`, `room_id`) VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8);",
                    (int)ticket.State, (int)ticket.Type, ticket.Category, ticket.Timestamp, ticket.Priority, ticket.SenderId, ticket.ReportedId, ticket.Caption, ticket.RoomId);
            });
            return ticketId;
        }

        public async Task UpdateTicket(IModerationTicket ticket)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `moderation_tickets` SET `state` = @1, `mod_id` = @2 WHERE `id` = @0;",
                    ticket.Id, (int)ticket.State, ticket.ModId);
            });
        }
    }
}
