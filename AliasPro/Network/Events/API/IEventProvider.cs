using System.Threading.Tasks;

namespace AliasPro.Network.Events
{
    using Protocol;
    using Sessions;

    public interface IEventProvider
    {
        Task Handle(ISession session, IClientPacket clientPacket);
    }
}