﻿namespace AliasPro.Items.Types
{
    public enum WiredTriggerType
    {
        DEFAULT = 0,
        SAY_SOMETHING = 0,
        WALKS_ON_FURNI = 1,
        WALKS_OFF_FURNI = 2,
        AT_GIVEN_TIME = 3,
        STATE_CHANGED = 4,
        PERIODICALLY = 6,
        ENTER_ROOM = 7,
        GAME_STARTS = 8,
        GAME_ENDS = 9,
        SCORE_ACHIEVED = 10,
        COLLISION = 11,
        PERIODICALLY_LONG = 12,

        BOT_REACHED_STF = 13,
        BOT_REACHED_AVTR = 14
    }
}
