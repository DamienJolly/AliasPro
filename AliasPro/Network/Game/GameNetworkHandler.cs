using AliasPro.API.Items;
using AliasPro.API.Messenger;
using AliasPro.API.Players;
using AliasPro.API.Sessions;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Protocols;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;

namespace AliasPro.Network.Game
{
	public class GameNetworkHandler : SimpleChannelInboundHandler<ClientMessage>
    {
        private readonly ILogger<GameNetworkHandler> logger;
        private readonly ISessionController sessionController;
        private readonly MessageHandler messageHandler;

        public GameNetworkHandler(
            ILogger<GameNetworkHandler> logger,
            ISessionController sessionController,
            MessageHandler messageHandler)
        {
            this.logger = logger;
            this.sessionController = sessionController;
            this.messageHandler = messageHandler;
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            if (!sessionController.TryGetSession(context.Channel.Id, out _))
            {
                sessionController.CacheSession(context);
            }
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            if (sessionController.TryGetSession(context.Channel.Id, out ISession session))
            {
                DisconnectPlayer(session);
            }
            sessionController.RemoveFromCache(context.Channel.Id);
        }

        protected override async void ChannelRead0(IChannelHandlerContext context, ClientMessage msg)
        {
            if (sessionController.TryGetSession(context.Channel.Id, out ISession session))
            {
                await messageHandler.TryHandleAsync(session, msg);
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            throw exception;
        }
        
        private async void DisconnectPlayer(ISession session)
        {
            if (session.Player == null)
                return;

            session.Player.Online = false;

            await Program.GetService<IPlayerController>().UpdatePlayerAsync(session.Player);
            await Program.GetService<IPlayerController>().UpdatePlayerSettingsAsync(session.Player);
            await Program.GetService<IPlayerController>().UpdatePlayerCurrenciesAsync(session.Player);
            await Program.GetService<IPlayerController>().UpdatePlayerBadgesAsync(session.Player);

            if (session.Player.Inventory != null)
                await Program.GetService<IItemController>().UpdatePlayerItemsAsync(session.Player.Inventory.Items);

            if (session.CurrentRoom != null && session.Entity != null)
                await session.CurrentRoom.RemoveEntity(session.Entity, false);

            if (session.Player.Messenger != null)
                await Program.GetService<IMessengerController>().UpdateStatusAsync(session.Player, session.Player.Messenger.Friends);

            Program.GetService<IPlayerController>().RemovePlayer(session.Player);
        }
    }
}
