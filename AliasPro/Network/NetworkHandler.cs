using AliasPro.Network.Protocol;
using AliasPro.Sessions;
using DotNetty.Transport.Channels;
using System;

namespace AliasPro.Network
{
    internal class NetworkHandler : SimpleChannelInboundHandler<IClientPacket>
    {
        private readonly ISessionController _sessionController;

        internal NetworkHandler(
            ISessionController sessionController)
        {
            _sessionController = sessionController;
        }

        public override void ChannelRegistered(IChannelHandlerContext context) =>
            _sessionController.CacheSession(context);

        public override void ChannelUnregistered(IChannelHandlerContext context) =>
            _sessionController.RemoveFromCache(context.Channel.Id);

        protected override async void ChannelRead0(IChannelHandlerContext ctx, IClientPacket msg)
        {
            if (_sessionController.TryGetSession(ctx.Channel.Id, out ISession session))
            {
                //todo: handle packet
            }
        }
    }
}
