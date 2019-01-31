using System.Threading.Tasks;

namespace AliasPro.Network.Events
{
    using Protocol;
    using Sessions;

    public interface IAsyncPacket
    {
        short Header { get; }
        Task HandleAsync(ISession session, IClientPacket clientPacket);
    }
}
