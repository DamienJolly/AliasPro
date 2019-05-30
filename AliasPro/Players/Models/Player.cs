using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Players.Components;
using AliasPro.Players.Cycles;

namespace AliasPro.Players.Models
{
	internal class Player : PlayerData, IPlayer
	{
		public ISession Session { get; set; }
		public IPlayerSettings PlayerSettings { get; set; }

        public MessengerComponent Messenger { get; set; }
        public CurrencyComponent Currency { get; set; }
        public BadgeComponent Badge { get; set; }
		public AchievementComponent Achievement { get; set; }
		public InventoryComponent Inventory { get; set; }

        public PlayerCycle PlayerCycle { get; set; }

        internal Player(ISession session, IPlayerData data)
            : base(data)
        {
            Session = session;
        }
	}
}
