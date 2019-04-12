namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    using AliasPro.Items.Models;

    public class WiredInteractor
    {
        public static IWiredInteractor GetWiredInteractor(WiredInteraction interaction, IItem item)
        {
            switch (interaction)
            {
                case WiredInteraction.DEFAULT: default: return new WiredInteractionDefault(item);

                    // Triggers
                case WiredInteraction.REPEATER: return new WiredInteractionRepeater(item);
                case WiredInteraction.WALKS_ON_FURNI: return new WiredInteractionWalksOn(item);
                case WiredInteraction.WALKS_OFF_FURNI: return new WiredInteractionWalksOff(item);
                case WiredInteraction.STATE_CHANGED: return new WiredInteractionStateChanged(item);
                case WiredInteraction.SAY_SOMETHING: return new WiredInteractionSaysSomething(item);
                case WiredInteraction.GAME_STARTS: return new WiredInteractionGameStarts(item);
                case WiredInteraction.GAME_ENDS: return new WiredInteractionGameEnds(item);
                case WiredInteraction.COLLISION: return new WiredInteractionCollision(item);
                case WiredInteraction.REPEATER_LONG: return new WiredInteractionRepeaterLong(item);
                case WiredInteraction.ENTER_ROOM: return new WiredInteractionEnterRoom(item);
                case WiredInteraction.SCORE_ACHIEVED: return new WiredInteractionScoreAchieved(item);
                case WiredInteraction.AT_GIVEN_TIME: return new WiredInteractionAtGivenTime(item);

                    // Effects
                case WiredInteraction.MESSAGE: return new WiredInteractionMessage(item);
                case WiredInteraction.TOGGLE_STATE: return new WiredInteractionToggleState(item);
                case WiredInteraction.TELEPORT: return new WiredInteractionTeleport(item);
                case WiredInteraction.MOVE_ROTATE: return new WiredInteractionMoveRotate(item);
                case WiredInteraction.LEAVE_TEAM: return new WiredInteractionLeaveTeam(item);
                case WiredInteraction.JOIN_TEAM: return new WiredInteractionJoinTeam(item);
                case WiredInteraction.GIVE_SCORE: return new WiredInteractionGiveScore(item);
                case WiredInteraction.GIVE_SCORE_TEAM: return new WiredInteractionGiveScoreTeam(item);
                case WiredInteraction.MATCH_POSITION: return new WiredInteractionMatchPosition(item);
                case WiredInteraction.MOVE_DIRECTION: return new WiredInteractionMoveDirection(item);
                case WiredInteraction.CHASE: return new WiredInteractionChase(item);
                case WiredInteraction.FLEE: return new WiredInteractionFlee(item);
                case WiredInteraction.RESET_TIMERS: return new WiredInteractionResetTimers(item);
                case WiredInteraction.CALL_STACKS: return new WiredInteractionTriggerStacks(item);

                    // Conditions
                case WiredInteraction.ACTOR_IN_TEAM: return new WiredInteractionActorInTeam(item);
            }
        }
    }

    public interface IWiredInteractor
    {
        WiredData WiredData { get; set; }
        bool OnTrigger(params object[] args);
        void OnCycle();
    }
}
