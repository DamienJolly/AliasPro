namespace AliasPro.Items.Types
{
    public enum WiredInteractionType
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
        CALL_STACKS = 26,

        // Conditions
        MATCH_SSHOT = 27,
        FURNI_HAVE_HABBO = 28,
        TRIGGER_ON_FURNI = 29,
        TIME_MORE_THAN = 30,
        TIME_LESS_THAN = 31,
        USER_COUNT = 32,
        ACTOR_IN_TEAM = 33,
        FURNI_HAS_FURNI = 34,
        STUFF_IS = 35,
        ACTOR_IN_GROUP = 36, //?
        ACTOR_WEARS_BADGE = 37,
        ACTOR_WEARS_EFFECT = 38,
        NOT_MATCH_SSHOT = 39, 
        NOT_FURNI_HAVE_HABBO = 40,
        NOT_ACTOR_ON_FURNI = 41,
        NOT_USER_COUNT = 42,
        NOT_ACTOR_IN_TEAM = 43,
        NOT_FURNI_HAVE_FURNI = 44,
        NOT_STUFF_IS = 45,
        NOT_ACTOR_IN_GROUP = 46,
        NOT_ACTOR_WEARS_BADGE = 47,
        NOT_ACTOR_WEARS_EFFECT = 48,
        DATE_RANGE = 49,
        ACTOR_HAS_HANDITEM = 50

        // Other
    }
}
