using AliasPro.API.Catalog.Models;
using AliasPro.API.Items.Models;
using AliasPro.Network.Protocol;
using AliasPro.Sessions;

namespace AliasPro.API.Catalog.Layouts
{
    public interface ICatalogLayout
    {
        void Compose(ServerPacket message);
        IItem HandlePurchase(ICatalogItemData catalogItem, ISession session, string extraData);
    }
}
