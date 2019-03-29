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
        REPEATER_LONG = 17,
        ENTER_ROOM = 18,
        SCORE_ACHIEVED = 19,
        AT_GIVEN_TIME = 20,

        // Effects
        MESSAGE = 2,
        TOGGLE_STATE = 9,
        TELEPORT = 10,
        MOVE_ROTATE = 11,
        LEAVE_TEAM = 13,
        JOIN_TEAM = 14,
        GIVE_SCORE = 15,
        GIVE_SCORE_TEAM = 16,
        MATCH_POSITION = 21,
        MOVE_DIRECTION = 22,
        CHASE = 23,
        FLEE = 24,
        RESET_TIMERS = 25,
        CALL_STACKS = 26

        // Conditions

        // Other
    }
}
