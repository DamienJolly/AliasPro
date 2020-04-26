using AliasPro.API.Moderation;
using AliasPro.API.Moderation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Moderation
{
    internal class ModerationController : IModerationController
    {
        private readonly ModerationDao _moderationDao;

		private IList<IModerationPreset> _presets;
		private IDictionary<int, IModerationTicket> _modTickets;
		private IDictionary<int, IModerationCategory> _modCategory;

        public ModerationController(ModerationDao moderationDao)
        {
            _moderationDao = moderationDao;

			_presets = new List<IModerationPreset>();
			_modTickets = new Dictionary<int, IModerationTicket>();
			_modCategory = new Dictionary<int, IModerationCategory>();

			InitializeModeration();
        }

        public async void InitializeModeration()
        {
			_presets = await _moderationDao.ReadPresets();
			_modTickets = await _moderationDao.ReadTickets();
			_modCategory = await _moderationDao.ReadCategories();
        }

        public ICollection<IModerationTicket> Tickets =>
            _modTickets.Values;

		public ICollection<IModerationCategory> Categories =>
			_modCategory.Values;

		public ICollection<IModerationPreset> GetPresets(string type)
		{
			ICollection<IModerationPreset> presets = new List<IModerationPreset>();

			foreach (IModerationPreset preset in _presets)
				if (preset.Type == type) presets.Add(preset);

			return presets;
		}

		public bool TryAddTicket(IModerationTicket ticket) =>
			_modTickets.TryAdd(ticket.Id, ticket);

		public bool TryGetTicket(int ticketId, out IModerationTicket ticket) =>
            _modTickets.TryGetValue(ticketId, out ticket);

		public bool TryGetTopic(int topicId, out IModerationTopic topic)
		{
			topic = null;
			foreach (IModerationCategory category in _modCategory.Values)
			{
				if (category.TryGetTopic(topicId, out topic))
					return true;
			}
			return false;
		}

		public async Task<int> AddTicket(IModerationTicket ticket) =>
			await _moderationDao.AddTicket(ticket);

		public Task UpdateTicket(IModerationTicket ticket) =>
            _moderationDao.UpdateTicket(ticket);

        public void RemoveTicket(int ticketId) =>
            _modTickets.Remove(ticketId);
	}
}
