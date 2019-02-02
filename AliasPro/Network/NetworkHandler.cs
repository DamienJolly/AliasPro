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

        public override async void ChannelUnregistered(IChannelHandlerContext context)
        {
            if (_sessionController.TryGetSession(context.Channel.Id, out ISession session))
            {
                await session.Disconnect(false);
            }
            _sessionController.RemoveFromCache(context.Channel.Id);
        }

        protected override async void ChannelRead0(IChannelHandlerContext context, IClientPacket msg)
        {
            if (_sessionController.TryGetSession(context.Channel.Id, out ISession session))
            {
                await _eventProvider.Handle(session, msg);
            }
        }
    }
}
