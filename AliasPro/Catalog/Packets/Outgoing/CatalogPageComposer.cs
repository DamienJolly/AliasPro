namespace AliasPro.Catalog.Packets.Outgoing
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class CatalogPageComposer : IPacketComposer
    {
        private readonly ICatalogPage _catalogPage;
        private readonly string _mode;

        public CatalogPageComposer(ICatalogPage catalogPage, string mode)
        {
            _catalogPage = catalogPage;
            _mode = mode;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.CatalogPageMessageComposer);
            message.WriteInt(_catalogPage.Id);
            message.WriteString(_mode);
            _catalogPage.Layout.Compose(message, _catalogPage);

            message.WriteInt(_catalogPage.Items.Count);
            foreach (ICatalogItem catalogitem in _catalogPage.Items)
            {
                message.WriteInt(catalogitem.Id);
                message.WriteString(catalogitem.Name);
                message.WriteBoolean(false);
                message.WriteInt(catalogitem.Credits);
                message.WriteInt(catalogitem.Points);
                message.WriteInt(catalogitem.PointsType);
                message.WriteBoolean(catalogitem.CanGift);

                message.WriteInt(catalogitem.Items.Count);
                foreach (var item in catalogitem.Items)
                {
                    message.WriteString(item.ItemData.Type);
                    message.WriteInt(item.ItemData.SpriteId);
                    message.WriteString(item.ItemData.ExtraData);
                    message.WriteInt(item.Amount);

                    message.WriteBoolean(catalogitem.IsLimited && item.Amount <= 1);
                    if (catalogitem.IsLimited && item.Amount <= 1)
                    {
                        message.WriteInt(catalogitem.LimitedStack);
                        message.WriteInt(catalogitem.LimitedStack - catalogitem.LimitedSells);
                    }
                }

                message.WriteInt(catalogitem.ClubLevel);
                message.WriteBoolean(catalogitem.HasOffer);
                message.WriteBoolean(false);
                message.WriteString(catalogitem.Name + ".png");
            }

            message.WriteInt(0);
            message.WriteBoolean(false);
            return message;
        }
    }
}
