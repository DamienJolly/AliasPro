using AliasPro.API.Catalog.Models;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Protocol;

namespace AliasPro.API.Catalog.Layouts
{
    public interface ICatalogLayout
    {
        void Compose(ServerPacket message);
        IItem HandlePurchase(ICatalogItemData catalogItem, ISession session, string extraData);
    }
}
