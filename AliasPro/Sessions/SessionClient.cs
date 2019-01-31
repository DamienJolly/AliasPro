using DotNetty.Transport.Channels;
using System.Threading.Tasks;

namespace AliasPro.Sessions
{
    using Room.Models;
    using Player.Models;
    using Network.Protocol;

    internal class SessionClient : ISession
    {
        public string UniqueId { get; set; }
        public IPlayer Player { get; set; }
        public IRoom CurrentRoom { get; set; }

        private readonly IChannelHandlerContext _channel;

        internal SessionClient(IChannelHandlerContext context)
        {
            _channel = context;
        }

        public Task WriteAsync(ServerPacket serverPacket) => _channel.WriteAsync(serverPacket);

        public Task WriteAndFlushAsync(ServerPacket serverPacket) => _channel.WriteAndFlushAsync(serverPacket);

        public void Flush() => _channel.Flush();

        public Task CloseAsync() => _channel.CloseAsync();
    }
}
