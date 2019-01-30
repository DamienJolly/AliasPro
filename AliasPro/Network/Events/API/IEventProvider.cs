using AliasPro.Network.Protocol;
using AliasPro.Sessions;
using System.Threading.Tasks;

namespace AliasPro.Network.Events
{
    public interface IEventProvider
    {
        Task Handle(ISession session, IClientPacket clientPacket);
    }
}