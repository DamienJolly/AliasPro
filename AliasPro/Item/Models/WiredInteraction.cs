namespace AliasPro.Item.Models
{
    public enum WiredInteraction
    {
        DEFAULT = 0,

        // Triggers
        REPEATER = 1,
        WALKS_ON_FURNI = 3,
        WALKS_OFF_FURNI = 4,
        STATE_CHANGED = 5,
        SAY_SOMETHING = 6,
        GAME_STARTS = 7,
        GAME_ENDS = 8,
        COLLISION = 12,

        // Effects
        MESSAGE = 2,
        TOGGLE_STATE = 9,
        TELEPORT = 10,
        MOVE_ROTATE = 11

        // Conditions

        // Other
    }
}
