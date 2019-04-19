using AliasPro.API.Chat.Models;
using AliasPro.API.Sessions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Chat
{
    public interface IChatController
    {
        bool HandleCommand(ISession session, string message);
        Task<ICollection<IChatLog>> ReadRoomChatlogs(uint roomId);
        Task<ICollection<IChatLog>> ReadUserChatlogs(uint playerId);
    }
}
