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
                case "roller": return ItemInteractionType.ROLLER;
                case "dice": return ItemInteractionType.DICE;
				case "exchange": return ItemInteractionType.EXCHANGE;
				case "bed": return ItemInteractionType.BED;
                case "chair": return ItemInteractionType.CHAIR;
                case "default": default: return ItemInteractionType.DEFAULT;
            }
        }
    }
}
