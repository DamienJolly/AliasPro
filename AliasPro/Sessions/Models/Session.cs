using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models;
using AliasPro.Room.Models.Entities;
using DotNetty.Transport.Channels;
using System.Threading.Tasks;

namespace AliasPro.Sessions.Models
{
    internal class Session : ISession
    {
        public string UniqueId { get; set; }
        public IPlayer Player { get; set; }
        public BaseEntity Entity { get; set; }
        public IRoom CurrentRoom { get; set; }

        private readonly IChannelHandlerContext _channel;

        internal Session(IChannelHandlerContext context)
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
}
