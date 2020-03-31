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

        public ModerationController(ModerationDao moderationDao)
        {
            _moderationDao = moderationDao;

			_presets = new List<IModerationPreset>();
			_modTickets = new Dictionary<int, IModerationTicket>();

			InitializeModeration();
        }

        public async void InitializeModeration()
        {
			_presets = await _moderationDao.ReadPresets();
			_modTickets = await _moderationDao.ReadTickets();
        }

        public ICollection<IModerationTicket> Tickets =>
            _modTickets.Values;

		public ICollection<IModerationPreset> GetPresets(string type)
		{
			ICollection<IModerationPreset> presets = new List<IModerationPreset>();

			foreach (IModerationPreset preset in _presets)
				if (preset.Type == type) presets.Add(preset);

			return presets;
		}

		public bool TryGetTicket(int ticketId, out IModerationTicket ticket) =>
            _modTickets.TryGetValue(ticketId, out ticket);

        public Task UpdateTicket(IModerationTicket ticket) =>
            _moderationDao.UpdateTicket(ticket);

        public void RemoveTicket(int ticketId) =>
            _modTickets.Remove(ticketId);
	}
}
