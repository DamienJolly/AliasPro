﻿using DotNetty.Transport.Channels;
using System.Threading.Tasks;

namespace AliasPro.Sessions
{
    using Room.Models;
    using Room.Models.Entities;
    using Player.Models;
    using Network.Events;
    using Network.Protocol;

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

        public Task SendPacketAsync(IPacketComposer serverPacket) => WriteAndFlushAsync(serverPacket.Compose());
            
        private Task WriteAndFlushAsync(ServerPacket serverPacket) => _channel.WriteAndFlushAsync(serverPacket);

        public async Task Disconnect(bool closeSocket)
        {
            if (Entity != null)
            {
                CurrentRoom.LeaveRoom(this);
            }
            if (closeSocket)
            {
                await _channel.CloseAsync();
            }
        }
    }
}
