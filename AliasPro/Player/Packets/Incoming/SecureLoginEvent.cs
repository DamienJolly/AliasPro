using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models;
    using Packets.Outgoing;
    using Sessions;
    using Item;
    using AliasPro.Player.Models.Currency;

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
            IPlayer player = await _playerController.GetPlayerBySsoAsync(ssoTicket);
            if (player != null)
            {
                player.Session = session;
                session.Player = player;
                IPlayerSettings playerSettings =
                    await _playerController.GetPlayerSettingsByIdAsync(player.Id);

                if (playerSettings == null)
                {
                    await _playerController.AddPlayerSettingsAsync(player.Id);
                    playerSettings =
                        await _playerController.GetPlayerSettingsByIdAsync(player.Id);
                }

                session.Player.PlayerSettings = playerSettings;

                session.Player.Currency = new PlayerCurrency(
                    await _playerController.GetPlayerCurrenciesByIdAsync(player.Id));

                session.Player.Inventory = new PlayerInventory(session,
                    await _itemController.GetItemsForPlayerAsync(session.Player.Id));

                await session.SendPacketAsync(new SecureLoginOKComposer());
                await session.SendPacketAsync(new HomeRoomComposer(1));

                await session.SendPacketAsync(new UserRightsComposer(session.Player));
                await session.SendPacketAsync(new AvailabilityStatusComposer());
            }
        }
    }
}
