using AliasPro.API.Moderation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Moderation
{
    public interface IModerationController
    {
        void InitializeModeration();
		ICollection<IModerationTicket> Tickets { get; }
		ICollection<IModerationCategory> Categories { get; }

		ICollection<IModerationPreset> GetPresets(string type);
		bool TryAddTicket(IModerationTicket ticket);
		bool TryGetTicket(int ticketId, out IModerationTicket ticket);
        Task UpdateTicket(IModerationTicket ticket);
		Task<int> AddTicket(IModerationTicket ticket);

		void RemoveTicket(int ticketId);
		bool TryGetTopic(int topicId, out IModerationTopic topic);
	}
}
