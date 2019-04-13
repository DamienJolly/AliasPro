using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Room.Models;
using AliasPro.Room.Models.Entities;
using System.Threading.Tasks;

namespace AliasPro.API.Sessions.Models
{
    public interface ISession
    {
        string UniqueId { get; set; }
        IPlayer Player { get; set; }
        BaseEntity Entity { get; set; }
        IRoom CurrentRoom { get; set; }

        void Disconnect();
        Task SendPacketAsync(IPacketComposer serverPacket);
    }
}
