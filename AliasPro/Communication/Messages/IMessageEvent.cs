using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Communication.Messages
{
    public interface IMessageEvent
    {
        short Id { get; }

        Task RunAsync(ISession session, ClientMessage message);
    }
}
