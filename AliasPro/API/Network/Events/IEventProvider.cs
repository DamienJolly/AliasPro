using AliasPro.API.Network.Protocol;
using AliasPro.Sessions;

namespace AliasPro.API.Network.Events
{
    public interface IEventProvider
    {
        void Handle(ISession session, IClientPacket clientPacket);
    }
}
