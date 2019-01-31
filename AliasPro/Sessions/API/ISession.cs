using System.Threading.Tasks;

namespace AliasPro.Sessions
{
    using Room.Models;
    using Room.Models.Entities;
    using Player.Models;
    using Network.Protocol;

    public interface ISession
    {
        string UniqueId { get; set; }
        IPlayer Player { get; set; }
        BaseEntity Entity { get; set; }
        IRoom CurrentRoom { get; set; }
        Task WriteAsync(ServerPacket serverPacket);
        Task WriteAndFlushAsync(ServerPacket serverPacket);
        void Flush();
        Task CloseAsync();
    }
}
