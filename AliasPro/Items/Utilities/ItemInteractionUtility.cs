using AliasPro.Items.Types;

namespace AliasPro.Items.Utilities
{
    public class ItemInteractionUtility
    {
        public static ItemInteractionType GetItemInteractor(string interaction)
        {
            switch (interaction)
            {
                case "wired_trigger": return ItemInteractionType.WIRED_TRIGGER;
                case "wired_effect": return ItemInteractionType.WIRED_EFFECT;
                case "wired_condition": return ItemInteractionType.WIRED_CONDITION;
                case "game_timer": return ItemInteractionType.GAME_TIMER;
                case "vending": return ItemInteractionType.VENDING_MACHINE;
				case "love_lock": return ItemInteractionType.LOVE_LOCK;
				case "badge_display": return ItemInteractionType.BADGE_DISPLAY;
				case "multiheight": return ItemInteractionType.MULTIHEIGHT;
				case "onewaygate": return ItemInteractionType.ONE_WAY_GATE;
				case "bottle": return ItemInteractionType.BOTTLE;
				case "roller": return ItemInteractionType.ROLLER;
                case "dice": return ItemInteractionType.DICE;
				case "exchange": return ItemInteractionType.EXCHANGE;
				case "bed": return ItemInteractionType.BED;
				case "gate": return ItemInteractionType.GATE;
				case "chair": return ItemInteractionType.CHAIR;
                case "default": default: return ItemInteractionType.DEFAULT;
            }
        }
    }
}
