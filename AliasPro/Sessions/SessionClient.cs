using DotNetty.Transport.Channels;
using System.Threading.Tasks;

namespace AliasPro.Sessions
{
    using Room.Models;
    using Room.Models.Entities;
    using Network.Events;
    using Network.Protocol;
    using AliasPro.API.Player.Models;

    internal class SessionClient : ISession
    {
        public string UniqueId { get; set; }
        public IPlayer Player { get; set; }
        public BaseEntity Entity { get; set; }
        public IRoom CurrentRoom { get; set; }

        private readonly IChannelHandlerContext _channel;

        internal SessionClient(IChannelHandlerContext context)
        {
            _channel = context;
        }

        public void Disconnect()
        {
            _channel.CloseAsync();
        }

        public Task SendPacketAsync(IPacketComposer serverPacket) => WriteAndFlushAsync(serverPacket.Compose());
            
        private Task WriteAndFlushAsync(ServerPacket serverPacket) => _channel.WriteAndFlushAsync(serverPacket);
    }

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
