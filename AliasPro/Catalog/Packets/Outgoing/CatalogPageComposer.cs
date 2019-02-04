using System.Collections.Generic;

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

            if (_catalogPage.Layout == "frontpage")
            {
                message.WriteString("frontpage4");
                message.WriteInt(2);
                message.WriteString(_catalogPage.HeaderImage);
                message.WriteString(_catalogPage.TeaserImage);
                message.WriteInt(3);
                message.WriteString(_catalogPage.TextOne);
                message.WriteString(_catalogPage.TextTwo);
                message.WriteString(_catalogPage.TextTeaser);
            }
            else
            {
                message.WriteString("default_3x3");
                message.WriteInt(3);
                message.WriteString(_catalogPage.HeaderImage);
                message.WriteString(_catalogPage.TeaserImage);
                message.WriteString(_catalogPage.SpecialImage);
                message.WriteInt(3);
                message.WriteString(_catalogPage.TextOne);
                message.WriteString(_catalogPage.TextDetails);
                message.WriteString(_catalogPage.TextTeaser);
            }

            message.WriteInt(0);

            message.WriteInt(0);
            message.WriteBoolean(false);
            return message;
        }
    }
}
