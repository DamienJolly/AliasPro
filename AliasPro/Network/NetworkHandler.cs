using DotNetty.Transport.Channels;

namespace AliasPro.Network
{
    using Events;
    using Protocol;
    using Sessions;

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
