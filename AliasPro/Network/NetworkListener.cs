using AliasPro.API.Network;
using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AliasPro.Network
{
    internal class NetworkListener : INetworkListener
    {
        private IEventLoopGroup _workerGroup;
        private IEventLoopGroup _bossGroup;

        private readonly ILogger<NetworkListener> _logger;
        private readonly NetworkInitializer _initializer;

        public NetworkListener(ILogger<NetworkListener> logger, NetworkInitializer initalizer)
        {
            _logger = logger;
            _initializer = initalizer;
        }

        public async Task Listen(int port)
        {
            _workerGroup = new MultithreadEventLoopGroup(10);
            _bossGroup = new MultithreadEventLoopGroup(1);

            ServerBootstrap bootstrap = new ServerBootstrap()
                .Group(_bossGroup, _workerGroup)
                .Channel<TcpServerSocketChannel>()
                .ChildHandler(_initializer)
                .ChildOption(ChannelOption.TcpNodelay, true)
                .ChildOption(ChannelOption.SoKeepalive, true)
                .ChildOption(ChannelOption.SoReuseaddr, true)
                .ChildOption(ChannelOption.SoRcvbuf, 1024)
                .ChildOption(ChannelOption.RcvbufAllocator, new FixedRecvByteBufAllocator(1024))
                .ChildOption(ChannelOption.Allocator, UnpooledByteBufferAllocator.Default);

            try
            {
                IChannel serverChannl = await bootstrap.BindAsync(new IPEndPoint(IPAddress.Parse("0.0.0.0"), port));
                if (!serverChannl.Active)
                    throw new Exception();

                _logger.LogInformation($"Listening on port: {port}");
            }
            catch
            {
                _logger.LogCritical($"Failed to listen on port: {port}");
            }
        }
    }
}
