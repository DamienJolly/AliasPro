using AliasPro.API.Items.Models;
using AliasPro.Items.Interaction.Wired;
using AliasPro.Items.Types;

namespace AliasPro.Items.Utilities
{
    public class WiredInteractorUtility
    {
        public static WiredInteraction GetWiredInteractor(WiredInteractionType interaction, IItem item)
        {
            switch (interaction)
            {
                case WiredInteractionType.DEFAULT: default: return new WiredInteractionDefault(item);

                // Triggers
                case WiredInteractionType.REPEATER: return new WiredInteractionRepeater(item);
                case WiredInteractionType.WALKS_ON_FURNI: return new WiredInteractionWalksOn(item);
                case WiredInteractionType.WALKS_OFF_FURNI: return new WiredInteractionWalksOff(item);
                case WiredInteractionType.STATE_CHANGED: return new WiredInteractionStateChanged(item);
                case WiredInteractionType.SAY_SOMETHING: return new WiredInteractionSaysSomething(item);
                case WiredInteractionType.GAME_STARTS: return new WiredInteractionGameStarts(item);
                case WiredInteractionType.GAME_ENDS: return new WiredInteractionGameEnds(item);
                case WiredInteractionType.COLLISION: return new WiredInteractionCollision(item);
                case WiredInteractionType.REPEATER_LONG: return new WiredInteractionRepeaterLong(item);
                case WiredInteractionType.ENTER_ROOM: return new WiredInteractionEnterRoom(item);
                case WiredInteractionType.SCORE_ACHIEVED: return new WiredInteractionScoreAchieved(item);
                case WiredInteractionType.AT_GIVEN_TIME: return new WiredInteractionAtGivenTime(item);

                // Effects
                case WiredInteractionType.MESSAGE: return new WiredInteractionMessage(item);
                case WiredInteractionType.TOGGLE_STATE: return new WiredInteractionToggleState(item);
                case WiredInteractionType.TELEPORT: return new WiredInteractionTeleport(item);
                case WiredInteractionType.MOVE_ROTATE: return new WiredInteractionMoveRotate(item);
                case WiredInteractionType.LEAVE_TEAM: return new WiredInteractionLeaveTeam(item);
                case WiredInteractionType.JOIN_TEAM: return new WiredInteractionJoinTeam(item);
                case WiredInteractionType.GIVE_SCORE: return new WiredInteractionGiveScore(item);
                case WiredInteractionType.GIVE_SCORE_TEAM: return new WiredInteractionGiveScoreTeam(item);
                case WiredInteractionType.MATCH_POSITION: return new WiredInteractionMatchPosition(item);
                case WiredInteractionType.MOVE_DIRECTION: return new WiredInteractionMoveDirection(item);
                case WiredInteractionType.CHASE: return new WiredInteractionChase(item);
                case WiredInteractionType.FLEE: return new WiredInteractionFlee(item);
                case WiredInteractionType.RESET_TIMERS: return new WiredInteractionResetTimers(item);
                case WiredInteractionType.CALL_STACKS: return new WiredInteractionTriggerStacks(item);

                // Conditions
                case WiredInteractionType.ACTOR_HAS_HANDITEM: return new WiredInteractionPlayerHasHanditem(item);

                // Positive Conditions
                case WiredInteractionType.FURNI_HAS_FURNI: return new WiredInteractionFurniHaveFurni(item);
                case WiredInteractionType.ACTOR_WEARS_BADGE: return new WiredInteractionPlayerWearsBadge(item);
                case WiredInteractionType.ACTOR_IN_TEAM: return new WiredInteractionPlayerInTeam(item);
                case WiredInteractionType.TRIGGER_ON_FURNI: return new WiredInteractionPlayerOnFurni(item);
                case WiredInteractionType.ACTOR_WEARS_EFFECT: return new WiredInteractionPlayerHasEffect(item);
                case WiredInteractionType.USER_COUNT: return new WiredInteractionPlayerCount(item);
                case WiredInteractionType.FURNI_HAVE_HABBO: return new WiredInteractionFurniHavePlayer(item);
                case WiredInteractionType.STUFF_IS: return new WiredInteractionFurniTypeMatch(item);
                case WiredInteractionType.MATCH_SSHOT: return new WiredInteractionMatchStatePosition(item);
                case WiredInteractionType.ACTOR_IN_GROUP: return new WiredInteractionPlayerInGroup(item);

                // Negative Conditions
                case WiredInteractionType.NOT_FURNI_HAVE_FURNI: return new WiredInteractionNotFurniHaveFurni(item);
                case WiredInteractionType.NOT_ACTOR_WEARS_BADGE: return new WiredInteractionNotPlayerWearsBadge(item);
                case WiredInteractionType.NOT_ACTOR_IN_TEAM: return new WiredInteractionNotPlayerInTeam(item);
                case WiredInteractionType.NOT_ACTOR_ON_FURNI: return new WiredInteractionNotPlayerOnFurni(item);
                case WiredInteractionType.NOT_ACTOR_WEARS_EFFECT: return new WiredInteractionNotPlayerHasEffect(item);
                case WiredInteractionType.NOT_USER_COUNT: return new WiredInteractionNotPlayerCount(item);
                case WiredInteractionType.NOT_FURNI_HAVE_HABBO: return new WiredInteractionNotFurniHavePlayer(item);
                case WiredInteractionType.NOT_STUFF_IS: return new WiredInteractionNotFurniTypeMatch(item);
                case WiredInteractionType.NOT_MATCH_SSHOT: return new WiredInteractionNotMatchStatePosition(item);
                case WiredInteractionType.NOT_ACTOR_IN_GROUP: return new WiredInteractionNotPlayerInGroup(item);
            }
        }
    }
}
