using AliasPro.API.Moderation;
using AliasPro.API.Moderation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Moderation
{
    internal class ModerationController : IModerationController
    {
        private readonly ModerationDao _moderationDao;
        private IDictionary<int, IModerationTicket> _modTickets;

        public ModerationController(ModerationDao moderationDao)
        {
            _moderationDao = moderationDao;
            _modTickets = new Dictionary<int, IModerationTicket>();

            InitializeModeration();
        }

        public async void InitializeModeration()
        {
            _modTickets = await _moderationDao.ReadTickets();
        }

        public ICollection<IModerationTicket> Tickets =>
            _modTickets.Values;

        public bool TryGetTicket(int ticketId, out IModerationTicket ticket) =>
            _modTickets.TryGetValue(ticketId, out ticket);

        public Task UpdateTicket(IModerationTicket ticket) =>
            _moderationDao.UpdateTicket(ticket);
    }
}
