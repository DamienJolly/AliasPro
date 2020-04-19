using AliasPro.Items.Types;

namespace AliasPro.Items.Utilities
{
    public class ItemInteractionUtility
    {
        public static ItemInteractionType GetItemInteractor(string interaction)
        {
            switch (interaction)
            {
				default: return ItemInteractionType.DEFAULT;
				case "default": return ItemInteractionType.DEFAULT;
				case "wired_trigger": return ItemInteractionType.WIRED_TRIGGER;
                case "wired_effect": return ItemInteractionType.WIRED_EFFECT;
                case "wired_condition": return ItemInteractionType.WIRED_CONDITION;
                case "game_timer": return ItemInteractionType.GAME_TIMER;
                case "vendingmachine": return ItemInteractionType.VENDING_MACHINE;
				case "lovelock": return ItemInteractionType.LOVE_LOCK;
				case "stacktool": return ItemInteractionType.STACK_TOOL;
				case "badge_display": return ItemInteractionType.BADGE_DISPLAY;
				case "multiheight": return ItemInteractionType.MULTIHEIGHT;
				case "onewaygate": return ItemInteractionType.ONE_WAY_GATE;
				case "clothing": return ItemInteractionType.CLOTHING;
				case "trophy": return ItemInteractionType.TROPHY;
				case "teleport": return ItemInteractionType.TELEPORT;
				case "bottle": return ItemInteractionType.BOTTLE;
				case "roller": return ItemInteractionType.ROLLER;
                case "dice": return ItemInteractionType.DICE;
				case "exchange": return ItemInteractionType.EXCHANGE;
				case "bed": return ItemInteractionType.BED;
				case "gate": return ItemInteractionType.GATE;
				case "chair": return ItemInteractionType.CHAIR;
				case "gift": return ItemInteractionType.GIFT;
				case "pressure_tile": return ItemInteractionType.PRESSURE_TILE;
				case "pressure_pad": return ItemInteractionType.PRESSURE_TILE;
				case "tent": return ItemInteractionType.TENT;
				case "water": return ItemInteractionType.WATER;
				case "wallpaper": return ItemInteractionType.WALLPAPER;
				case "floor": return ItemInteractionType.FLOOR;
				case "landscape": return ItemInteractionType.LANDSCAPE;
				case "roombg": return ItemInteractionType.BACKGROUND_TONER;
				case "ecotron": return ItemInteractionType.ECOTRON;
				case "dimmer": return ItemInteractionType.DIMMER;
				case "mannequin": return ItemInteractionType.MANNEQUIN;
				case "crackable": return ItemInteractionType.CRACKABLE;
				case "musicdisc": return ItemInteractionType.MUSICDISC;
				case "jukebox": return ItemInteractionType.JUKEBOX;
				case "ball": return ItemInteractionType.BALL;
				case "football_score": return ItemInteractionType.FOOTBALL_SCOREBOARD;
				case "football_goal": return ItemInteractionType.FOOTBALL_GOAL;
			}
        }
    }
}
