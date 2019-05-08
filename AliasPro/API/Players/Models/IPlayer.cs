using AliasPro.API.Sessions.Models;
using AliasPro.Players.Components;
namespace AliasPro.API.Players.Models
{
    public interface IPlayer : IPlayerData
    {
        ISession Session { get; set; }
        IPlayerSettings PlayerSettings { get; set; }

        MessengerComponent Messenger { get; set; }
        CurrencyComponent Currency { get; set; }
        BadgeComponent Badge { get; set; }
        InventoryComponent Inventory { get; set; }
    }
}
