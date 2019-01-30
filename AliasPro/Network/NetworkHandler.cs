using AliasPro.Network.Events;
using AliasPro.Network.Protocol;
using AliasPro.Sessions;
using DotNetty.Transport.Channels;

namespace AliasPro.Network
{
    internal class NetworkHandler : SimpleChannelInboundHandler<IClientPacket>
    {
        private readonly IEventProvider _eventProvider;
        private readonly ISessionController _sessionController;

        internal NetworkHandler(
            IEventProvider provider,
            ISessionController sessionController)
        {
            _eventProvider = provider;
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
                await _eventProvider.Handle(session, msg);
            }
        }
    }
}
