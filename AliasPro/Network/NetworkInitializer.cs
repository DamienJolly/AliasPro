using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace AliasPro.Network
{
    using Codec;
    using Events;
    using Sessions;
    using Player;
    using Room;

    internal class NetworkInitializer : ChannelInitializer<ISocketChannel>
    {
        private readonly IEventProvider _eventProvider;
        private readonly ISessionController _sessionController;
        private readonly IPlayerController _playerController;
        private readonly IRoomController _roomController;

        public NetworkInitializer(
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

        protected override void InitChannel(ISocketChannel channel)
        {
            channel.Pipeline
                .AddLast("encoder", new Encoder())
                .AddLast("decoder", new Decoder())
                .AddLast("handler", new NetworkHandler(_eventProvider, _sessionController, _playerController, _roomController));
        }
    }
}
