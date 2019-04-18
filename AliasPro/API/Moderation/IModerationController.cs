using AliasPro.API.Moderation.Models;
using System.Collections.Generic;

namespace AliasPro.API.Moderation
{
    public interface IModerationController
    {
        ICollection<IModerationTicket> Tickets { get; }
        void InitializeModeration();
    }
}
