using AliasPro.API.Figure;
using AliasPro.API.Items;
using AliasPro.API.Moderation;
using AliasPro.API.Permissions;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Players.Components;
using AliasPro.Players.Cycles;
using AliasPro.Players.Models;
using AliasPro.Players.Packets.Composers;
using AliasPro.Players.Types;
using AliasPro.Utilities;
using System;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class SecureLoginEvent : IMessageEvent
    {
        public short Header => Incoming.SecureLoginMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IItemController _itemController;
        private readonly IModerationController _moderationController;
		private readonly IFigureController _figureController;
		private readonly IPermissionsController _permissionsController;

        public SecureLoginEvent(
			IPlayerController playerController, 
			IItemController itemController, 
			IModerationController moderationController,
			IFigureController figureController,
			IPermissionsController permissionsController)
        {
            _playerController = playerController;
            _itemController = itemController;
            _moderationController = moderationController;
			_figureController = figureController;
			_permissionsController = permissionsController;
		}

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            string ssoTicket = message.ReadString();

            IPlayerData playerData = await _playerController.GetPlayerDataAsync(ssoTicket);

            if (playerData == null)
            {
                session.Disconnect();
                return;
            }

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

            player.Ignore = new IgnoreComponent(
                await _playerController.GetPlayerIgnoresAsync(player.Id));

            player.Inventory = new InventoryComponent(
                await _itemController.GetItemsForPlayerAsync(player.Id),
				await _playerController.GetPlayerBotsAsync(player.Id),
				await _playerController.GetPlayerPetsAsync(player.Id));

            player.Badge = new BadgeComponent(
                await _playerController.GetPlayerBadgesAsync(player.Id));

			player.Achievement = new AchievementComponent(
				await _playerController.GetPlayerAchievementsAsync(player.Id));

            player.Sanction = new SanctionComponent(
                await _playerController.GetPlayerSanctionsAsync(player.Id));

            player.Wardrobe = new WardrobeComponent(
				await _figureController.GetPlayerWardrobeAsync(player.Id),
				await _figureController.GetPlayerClothingAsync(player.Id));

			if (!_playerController.TryAddPlayer(player))
            {
                session.Disconnect();
                return;
            }

            if (player.Sanction.GetCurrentSanction(out IPlayerSanction sanction) && 
                sanction.Type == SanctionType.BAN)
            {
                session.Disconnect();
                return;
            }

            await session.SendPacketAsync(new SecureLoginOKComposer());
            await session.SendPacketAsync(new HomeRoomComposer(player.HomeRoom));

			await session.SendPacketAsync(new UserClothesComposer(player.Wardrobe.ClothingItems));
			await session.SendPacketAsync(new UserRightsComposer(player));
            await session.SendPacketAsync(new AvailabilityStatusComposer());
            await session.SendPacketAsync(new BuildersClubMembershipComposer());

			await session.SendPacketAsync(new ModerationTopicsComposer());

		    if (_permissionsController.HasPermission(session.Player, "acc_modtool"))
            {
                await session.SendPacketAsync(new ModerationToolComposer(
					_permissionsController,
					 player,
					_moderationController.GetPresets("user"),
					_moderationController.GetPresets("category"),
					_moderationController.GetPresets("room"),
					_moderationController.Tickets));
            }

            player.PlayerCycle = new PlayerCycle(player);
            player.Online = true;
            player.CheckLastOnline();

            await _playerController.UpdatePlayerAsync(player);
        }
    }
}
