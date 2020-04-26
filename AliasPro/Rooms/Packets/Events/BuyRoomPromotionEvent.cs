using AliasPro.API.Catalog;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Utilities;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class BuyRoomPromotionEvent : IMessageEvent
    {
        public short Header => Incoming.BuyRoomPromotionMessageEvent;

        private readonly ICatalogController _catalogController;
        private readonly IRoomController _roomController;

        public BuyRoomPromotionEvent(
            ICatalogController catalogController,
            IRoomController roomController)
        {
            _catalogController = catalogController;
            _roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int pageId = message.ReadInt();
            int itemId = message.ReadInt();
            int roomId = message.ReadInt();
            string title = message.ReadString();
            message.ReadBoolean(); //dunno??
            string description = message.ReadString();
            int categoryId = message.ReadInt();

            if (!_catalogController.TryGetCatalogPage(pageId, out ICatalogPage page))
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            if (page.Rank > session.Player.Rank)
            {
                await session.SendPacketAsync(new AlertPurchaseUnavailableComposer(AlertPurchaseUnavailableComposer.ILLEGAL));
                return;
            }

            if (!page.TryGetCatalogItem(itemId, out ICatalogItem catalogItem))
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            //todo: load room data
            if (!_roomController.TryGetRoom((uint)roomId, out IRoom roomData))
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            if (!roomData.Rights.HasRights(session.Player.Id))
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            if (catalogItem.Credits > 0)
            {
                if (session.Player.Currency.TryGetCurrency(-1, out IPlayerCurrency currency))
                {
                    if (catalogItem.Credits > currency.Amount)
                    {
                        // not enough credits
                        await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                        return;
                    }

                    currency.Amount -= catalogItem.Credits;
                    await session.SendPacketAsync(new UserCreditsComposer(currency.Amount));
                }
            }

            if (catalogItem.Points > 0)
            {
                if (session.Player.Currency.TryGetCurrency(catalogItem.PointsType, out IPlayerCurrency currency))
                {
                    if (catalogItem.Points > currency.Amount)
                    {
                        // not enough points
                        await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                        return;
                    }

                    currency.Amount -= catalogItem.Points;
                    await session.SendPacketAsync(new UserPointsComposer(currency.Amount, -catalogItem.Points, currency.Type));
                }
            }

            if (!roomData.IsPromoted || roomData.Promotion.Category != categoryId)
            {
                roomData.Promotion = new RoomPromotion(categoryId, title, description, (int)UnixTimestamp.Now, (int)UnixTimestamp.Now + (120 * 60));
                roomData.Promotion.Id = await _roomController.AddRoomPromotion(roomData.Id, roomData.Promotion);
            }
            else
            {
                roomData.Promotion.Title = title;
                roomData.Promotion.Description = description;
                roomData.Promotion.EndTimestamp += 120 * 60;
                await _roomController.UpdateRoomPromotion(roomData.Promotion);
            }

            await session.SendPacketAsync(new PurchaseOKComposer());

            if (_roomController.TryGetRoom(roomData.Id, out IRoom room))
            {
                room.Promotion = roomData.Promotion;
                await room.SendPacketAsync(new RoomPromotionComposer(room));
            }
        }
    }
}
