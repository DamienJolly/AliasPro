﻿using AliasPro.API.Sessions.Models;
using AliasPro.Players.Components;
using AliasPro.Players.Cycles;
using System.Threading.Tasks;

namespace AliasPro.API.Players.Models
{
    public interface IPlayer : IPlayerData
    {
        ISession Session { get; set; }
        IPlayerSettings PlayerSettings { get; set; }

        MessengerComponent Messenger { get; set; }
        CurrencyComponent Currency { get; set; }
        IgnoreComponent Ignore { get; set; }
        RecipeComponent Recipe { get; set; }
        BadgeComponent Badge { get; set; }
		AchievementComponent Achievement { get; set; }
        SanctionComponent Sanction { get; set; }
        InventoryComponent Inventory { get; set; }
		WardrobeComponent Wardrobe { get; set; }
        PlayerCycle PlayerCycle { get; set; }

        Task<IPlayerCurrency> GetPlayerCurrency(int type);
        void CheckLastOnline();
    }
}
