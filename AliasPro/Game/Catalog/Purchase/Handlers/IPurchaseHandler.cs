using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog.Purchase.Handlers
{
	public interface IPurchaseHandler
	{
		Task<bool> TryHandlePurchase(ISession session, string extraData, IItemData itemData, int amount = 1);
	}
}
