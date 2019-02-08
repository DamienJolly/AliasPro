using DotNetty.Transport.Channels;

namespace AliasPro.Network
{
    using Events;
    using Protocol;
    using Sessions;
    using Player;
    using Room;

    internal class NetworkHandler : SimpleChannelInboundHandler<IClientPacket>
    {
        private readonly IEventProvider _eventProvider;
        private readonly ISessionController _sessionController;
        private readonly IPlayerController _playerController;
        private readonly IRoomController _roomController;

        internal NetworkHandler(
            IEventProvider provider,
            ISessionController sessionController,
            IPlayerController playerController,
            IRoomController roomController)
        {
            _eventProvider = provider;
            _sessionController = sessionController;
            _playerController = playerController;
            _roomController = roomController;
        }

        public override void ChannelRegistered(IChannelHandlerContext context) =>
            _sessionController.CacheSession(context);

        public override async void ChannelUnregistered(IChannelHandlerContext context)
        {
            if (_sessionController.TryGetSession(context.Channel.Id, out ISession session) &&
                session.Player != null)
            {
                await _roomController.RemoveFromRoom(session);
                await _playerController.RemovePlayerByIdAsync(session.Player.Id);
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
