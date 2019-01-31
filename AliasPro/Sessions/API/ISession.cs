using System.Threading.Tasks;

namespace AliasPro.Sessions
{
    using Network.Protocol;

    public interface ISession
    {
        string UniqueId { get; set; }
        Task WriteAsync(ServerPacket serverPacket);
        Task WriteAndFlushAsync(ServerPacket serverPacket);
        void Flush();
        Task CloseAsync();
    }
}
