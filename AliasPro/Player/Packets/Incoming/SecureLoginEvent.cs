﻿using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models;
    using Packets.Outgoing;
    using Sessions;
    using Item;
    using AliasPro.API.Player.Models;
    using AliasPro.Player.Components;

    public class SecureLoginEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SecureLoginMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IItemController _itemController;

        public SecureLoginEvent(IPlayerController playerController, IItemController itemController)
        {
            _playerController = playerController;
            _itemController = itemController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string ssoTicket = clientPacket.ReadString();

            IPlayerData playerData = await _playerController.GetPlayerDataAsync(ssoTicket);

            if (_playerController.TryGetPlayer(playerData.Id, out IPlayer currentPlayer))
            {
                currentPlayer.Session.Disconnect();
            }
            
            IPlayer player = new Player(session, playerData);
            session.Player = player;

            IPlayerSettings playerSettings =
                    await _playerController.GetPlayerSettingsAsync(player.Id);

            if (playerSettings == null)
            {
                await _playerController.AddPlayerSettingsAsync(player.Id);
                playerSettings =
                    await _playerController.GetPlayerSettingsAsync(player.Id);
            }

            player.PlayerSettings = playerSettings;

            player.Currency = new CurrencyComponent(
                await _playerController.GetPlayerCurrenciesAsync(player.Id));

            player.Inventory = new InventoryComponent(
                await _itemController.GetItemsForPlayerAsync(player.Id));

            player.Badge = new BadgeComponent(
                await _playerController.GetPlayerBadgesAsync(player.Id));
            
            if (!_playerController.TryAddPlayer(player))
            {
                session.Disconnect();
                return;
            }

            await session.SendPacketAsync(new SecureLoginOKComposer());
            await session.SendPacketAsync(new HomeRoomComposer(0));

            await session.SendPacketAsync(new UserRightsComposer(player));
            await session.SendPacketAsync(new AvailabilityStatusComposer());

            //player.PlayerCycle = new PlayerCycle(_settingsController, player);
            //player.PlayerCycle.SetupPlayerCycle();

            await _playerController.UpdatePlayerAsync(player);
        }
    }
}
