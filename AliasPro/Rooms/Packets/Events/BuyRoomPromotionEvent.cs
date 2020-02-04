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
        public short Id { get; } = Incoming.BuyRoomPromotionMessageEvent;

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
            ClientMessage clientPacket)
        {
            int pageId = clientPacket.ReadInt();
            int itemId = clientPacket.ReadInt();
            int roomId = clientPacket.ReadInt();
            string title = clientPacket.ReadString();
            clientPacket.ReadBoolean(); //dunno??
            string description = clientPacket.ReadString();
            int categoryId = clientPacket.ReadInt();

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
                if (catalogItem.Credits > session.Player.Credits)
                {
                    // not enough credits
                    await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                    return;
                }

                session.Player.Credits -= catalogItem.Credits;
                await session.SendPacketAsync(new UserCreditsComposer(session.Player.Credits));
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
                //todo: add end timestamp to config?
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
