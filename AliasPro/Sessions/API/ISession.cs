using System.Threading.Tasks;

namespace AliasPro.Sessions
{
    using Room.Models;
    using Room.Models.Entities;
    using Player.Models;
    using Network.Events;

    public interface ISession
    {
        string UniqueId { get; set; }
        IPlayer Player { get; set; }
        BaseEntity Entity { get; set; }
        IRoom CurrentRoom { get; set; }
        Task SendPacketAsync(IPacketComposer serverPacket);
        Task CloseAsync();
    }
}
