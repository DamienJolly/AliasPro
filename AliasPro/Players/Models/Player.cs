﻿using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Players.Components;
using AliasPro.Players.Cycles;
using System.Threading.Tasks;

namespace AliasPro.Players.Models
{
	internal class Player : PlayerData, IPlayer
	{
		public ISession Session { get; set; }
		public IPlayerSettings PlayerSettings { get; set; }

        public MessengerComponent Messenger { get; set; }
        public CurrencyComponent Currency { get; set; }
        public IgnoreComponent Ignore { get; set; }
        public BadgeComponent Badge { get; set; }
		public AchievementComponent Achievement { get; set; }
		public InventoryComponent Inventory { get; set; }
		public WardrobeComponent Wardrobe { get; set; }

        public PlayerCycle PlayerCycle { get; set; }

        internal Player(ISession session, IPlayerData data)
            : base(data)
        {
            Session = session;
        }

        public async Task<IPlayerCurrency> GetPlayerCurrency(int type)
        {
            if (Currency == null)
                return null;

            if (!Currency.TryGetCurrency(type, out IPlayerCurrency currency))
            {
                currency = new PlayerCurrency(type);
                if (Currency.TryAddCurrency(currency))
                {
                    await Program.GetService<IPlayerController>().AddPlayerCurrencyAsync((int)Id, currency);
                }
            }

            return currency;
        }
	}
}
