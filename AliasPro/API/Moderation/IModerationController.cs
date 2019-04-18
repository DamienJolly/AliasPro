using AliasPro.API.Moderation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Moderation
{
    public interface IModerationController
    {
        ICollection<IModerationTicket> Tickets { get; }
        void InitializeModeration();
        bool TryGetTicket(int ticketId, out IModerationTicket ticket);
        Task UpdateTicket(IModerationTicket ticket);
        void RemoveTicket(int ticketId);
    }
}
