using AliasPro.API.Catalog.Layouts;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Models;
using AliasPro.Items.Types;
using System;

namespace AliasPro.Catalog.Layouts
{
    public class LayoutBadgeDisplay : ICatalogLayout
    {
        private readonly ICatalogPage _page;

        public LayoutBadgeDisplay(ICatalogPage page)
        {
            _page = page;
        }

        public void Compose(ServerMessage message)
        {
            message.WriteString("badge_display");
            message.WriteInt(3);
            message.WriteString(_page.HeaderImage);
            message.WriteString(_page.TeaserImage);
			message.WriteString(_page.SpecialImage);
			message.WriteInt(3);
            message.WriteString(_page.TextOne);
            message.WriteString(_page.TextDetails);
            message.WriteString(_page.TextTeaser);
        }

        public IItem HandleItemPurchase(ISession session, ICatalogItemData itemData, string extraData)
        {
			if (itemData.ItemData.InteractionType != ItemInteractionType.BADGE_DISPLAY)
				return null;

			if (!session.Player.Badge.HasBadge(extraData))
				return null;

			extraData = extraData + ";" + session.Player.Username + ";" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;

            return new Item((uint)itemData.Id, session.Player.Id, session.Player.Username, extraData, itemData.ItemData);
        }
    }
}
