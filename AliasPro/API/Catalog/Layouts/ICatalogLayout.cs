using AliasPro.API.Catalog.Models;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.API.Catalog.Layouts
{
    public interface ICatalogLayout
    {
        void Compose(ServerMessage message);
        IItem HandleItemPurchase(ISession session, ICatalogItemData itemData, string extraData);
    }
}
