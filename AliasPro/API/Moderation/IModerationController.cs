using AliasPro.API.Moderation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Moderation
{
    public interface IModerationController
    {
        void InitializeModeration();
		ICollection<IModerationTicket> Tickets { get; }
		ICollection<IModerationPreset> GetPresets(string type);
		bool TryGetTicket(int ticketId, out IModerationTicket ticket);
        Task UpdateTicket(IModerationTicket ticket);
        void RemoveTicket(int ticketId);
		Task AddPlayerSanction(uint playerId, string reason, int duration, int topicId);

	}
}
