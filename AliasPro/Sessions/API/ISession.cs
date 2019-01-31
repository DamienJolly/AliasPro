using AliasPro.Network.Protocol;
using System.Threading.Tasks;

namespace AliasPro.Sessions
{
    public interface ISession
    {
        string UniqueId { get; set; }
        Task WriteAsync(ServerPacket serverPacket);
        Task WriteAndFlushAsync(ServerPacket serverPacket);
        void Flush();
        Task CloseAsync();
    }
}
