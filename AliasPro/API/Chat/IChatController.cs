using AliasPro.API.Chat.Commands;
using AliasPro.API.Chat.Models;
using AliasPro.API.Sessions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Chat
{
    public interface IChatController
    {
		Task<bool> HandleCommand(ISession session, string message);
        Task<ICollection<IChatLog>> ReadUserChatlogs(uint playerId);
        Task<ICollection<IChatLog>> ReadRoomChatlogs(uint roomId, int enterTimestamp = 0, int exitTimestamp = int.MaxValue);
        IList<IChatCommand> CommandsForRank(ISession session);
    }
}
