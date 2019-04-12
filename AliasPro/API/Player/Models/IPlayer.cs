using AliasPro.Players.Components;
using AliasPro.Players.Models;
using AliasPro.Sessions;

namespace AliasPro.API.Player.Models
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
