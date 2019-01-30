using AliasPro.Network.Protocol;
using System.Threading.Tasks;

namespace AliasPro.Sessions
{
    public interface ISession
    {
        Task WriteAsync(ServerPacket serverPacket);
        Task WriteAndFlushAsync(ServerPacket serverPacket);
        void Flush();
        Task CloseAsync();
    }
}
