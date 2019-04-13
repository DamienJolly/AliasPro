using AliasPro.API.Items;
using AliasPro.API.Messenger;
using AliasPro.API.Network.Events;
using AliasPro.API.Players;
using AliasPro.API.Sessions;
using AliasPro.Network.Codec;
using AliasPro.Rooms;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace AliasPro.Network
{
    internal class NetworkInitializer : ChannelInitializer<ISocketChannel>
    {
        private readonly IEventProvider _eventProvider;
        private readonly ISessionController _sessionController;
        private readonly IPlayerController _playerController;
        private readonly IRoomController _roomController;
        private readonly IItemController _itemController;
        private readonly IMessengerController _messengerController;

        public NetworkInitializer(
            IEventProvider provider,
            ISessionController sessionController,
            IPlayerController playerController,
            IRoomController roomController,
            IItemController itemController,
            IMessengerController messengerController)
        {
            _eventProvider = provider;
            _sessionController = sessionController;
            _playerController = playerController;
            _roomController = roomController;
            _itemController = itemController;
            _messengerController = messengerController;
        }

        protected override void InitChannel(ISocketChannel channel)
        {
            channel.Pipeline
                .AddLast("encoder", new Encoder())
                .AddLast("decoder", new Decoder())
                .AddLast("handler", new NetworkHandler(
                    _eventProvider,
                    _sessionController, 
                    _playerController, 
                    _roomController, 
                    _itemController, 
                    _messengerController));
        }
    }
}
