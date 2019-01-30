using AliasPro.Network.Codec;
using AliasPro.Network.Events;
using AliasPro.Sessions;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace AliasPro.Network
{
    internal class NetworkInitializer : ChannelInitializer<ISocketChannel>
    {
        private readonly IEventProvider _eventProvider;
        private readonly ISessionController _sessionController;

        public NetworkInitializer(
            IEventProvider provider,
            ISessionController sessionController)
        {
            _eventProvider = provider;
            _sessionController = sessionController;
        }

        protected override void InitChannel(ISocketChannel channel)
        {
            channel.Pipeline
                .AddLast("encoder", new Encoder())
                .AddLast("decoder", new Decoder())
                .AddLast("handler", new NetworkHandler(_eventProvider, _sessionController));
        }
    }
}
