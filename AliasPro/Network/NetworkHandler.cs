using AliasPro.API.Figure;
using AliasPro.API.Items;
using AliasPro.API.Messenger;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Rooms;
using AliasPro.API.Sessions;
using AliasPro.API.Sessions.Models;
using DotNetty.Transport.Channels;

namespace AliasPro.Network
{
    internal class NetworkHandler : SimpleChannelInboundHandler<IClientPacket>
    {
        private readonly IEventProvider _eventProvider;
        private readonly ISessionController _sessionController;
        private readonly IPlayerController _playerController;
        private readonly IRoomController _roomController;
        private readonly IItemController _itemController;
        private readonly IMessengerController _messengerController;
        private readonly IFigureController _figureController;

        internal NetworkHandler(
            IEventProvider provider,
            ISessionController sessionController,
            IPlayerController playerController,
            IRoomController roomController,
            IItemController itemController,
            IMessengerController messengerController,
			IFigureController figureController)
        {
            _eventProvider = provider;
            _sessionController = sessionController;
            _playerController = playerController;
            _roomController = roomController;
            _itemController = itemController;
            _messengerController = messengerController;
			_figureController = figureController;
        }

        public override void ChannelRegistered(IChannelHandlerContext context) =>
            _sessionController.CacheSession(context);

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            if (_sessionController.TryGetSession(context.Channel.Id, out ISession session))
            {
                DisconnectPlayer(session);
            }
            _sessionController.RemoveFromCache(context.Channel.Id);
        }

        protected override void ChannelRead0(IChannelHandlerContext context, IClientPacket msg)
        {
            if (_sessionController.TryGetSession(context.Channel.Id, out ISession session))
            {
                _eventProvider.Handle(session, msg);
            }
        }

        private async void DisconnectPlayer(ISession session)
        {
            if (session.Player == null)
                return;
            
            session.Player.Online = false;

            await _playerController.UpdatePlayerAsync(session.Player);
            await _playerController.UpdatePlayerSettingsAsync(session.Player);
            await _playerController.UpdatePlayerCurrenciesAsync(session.Player);
            await _playerController.UpdatePlayerBadgesAsync(session.Player);

            if (session.Player.Inventory != null)
                await _itemController.UpdatePlayerItemsAsync(session.Player.Inventory.Items);

            if (session.Player.Messenger != null)
                await _messengerController.UpdateStatusAsync(session.Player, session.Player.Messenger.Friends);

			if (session.Player.Wardrobe != null)
				await _figureController.UpdateWardrobeItemsAsync(session.Player.Id, session.Player.Wardrobe.WardobeItems);

			if (session.CurrentRoom != null &&
                session.Entity != null)
                await session.CurrentRoom.RemoveEntity(session.Entity, false);
            
            _playerController.RemovePlayer(session.Player);
        }
    }
}
