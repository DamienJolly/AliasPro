using AliasPro.API.Players.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Protocols;
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

		public Task SendPacketAsync(IMessageComposer ServerMessage)
		{
			if (_channel.Channel.IsWritable)
				return WriteAndFlushAsync(ServerMessage.Compose());

			return Task.CompletedTask;
		}

		private Task WriteAndFlushAsync(ServerMessage ServerMessage) => _channel.WriteAndFlushAsync(ServerMessage);
    }
}
