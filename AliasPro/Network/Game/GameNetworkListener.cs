﻿using AliasPro.API.Sessions;
using AliasPro.Communication.Messages;
using AliasPro.Network.Game.Codecs;
using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AliasPro.Network.Game
{
	public class GameNetworkListener
	{
        private readonly ILogger<GameNetworkListener> logger;
        private readonly ILogger<GameNetworkHandler> handlerLogger;
        private readonly ISessionController sessionController;
        private readonly MessageHandler messageHandler;

        private ServerBootstrap bootstrap;
        private IEventLoopGroup bossGroup;
        private IEventLoopGroup workerGroup;

        public GameNetworkListener(
            ILogger<GameNetworkListener> logger, 
            ILogger<GameNetworkHandler> handlerLogger,
            ISessionController sessionController,
            MessageHandler messageHandler)
        {
            this.logger = logger;
            this.handlerLogger = handlerLogger;
            this.sessionController = sessionController;
            this.messageHandler = messageHandler;
        }

        public async Task StartAsync(int port)
        {
            bossGroup = new MultithreadEventLoopGroup(1);
            workerGroup = new MultithreadEventLoopGroup(10);

            bootstrap = new ServerBootstrap();
            bootstrap
                .Group(bossGroup, workerGroup)
                .Channel<TcpServerSocketChannel>()
                .ChildHandler(new ActionChannelInitializer<IChannel>(channel => {
                    channel.Pipeline.AddLast("encoder", new Encoder());
                    channel.Pipeline.AddLast("decoder", new Decoder());
                    channel.Pipeline.AddLast("handler", new GameNetworkHandler(
                        handlerLogger,
                        sessionController,
                        messageHandler));
                }))
                .ChildOption(ChannelOption.TcpNodelay, true)
                    .ChildOption(ChannelOption.SoKeepalive, true)
                    .ChildOption(ChannelOption.SoReuseaddr, true)
                    .ChildOption(ChannelOption.SoRcvbuf, 1024)
                    .ChildOption(ChannelOption.RcvbufAllocator, new FixedRecvByteBufAllocator(1024))
                    .ChildOption(ChannelOption.Allocator, UnpooledByteBufferAllocator.Default);

            await bootstrap.BindAsync(port);
            logger.LogInformation("Listening on port {0}, ready for connections", port);
        }

        public async Task StopAsync()
        {
            try
            {
                await workerGroup.ShutdownGracefullyAsync();
                await bossGroup.ShutdownGracefullyAsync();
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
