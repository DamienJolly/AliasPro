﻿namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    using AliasPro.Item.Models;

    public class WiredInteractor
    {
        public static IWiredInteractor GetWiredInteractor(WiredInteraction interaction, IItem item)
        {
            switch (interaction)
            {
                case WiredInteraction.COLLISION: return new WiredInteractionCollision(item);
                case WiredInteraction.MOVE_ROTATE: return new WiredInteractionMoveRotate(item);
                case WiredInteraction.TELEPORT: return new WiredInteractionTeleport(item);
                case WiredInteraction.TOGGLE_STATE: return new WiredInteractionToggleState(item);
                case WiredInteraction.MESSAGE: return new WiredInteractionMessage(item);
                case WiredInteraction.WALKS_ON_FURNI: return new WiredInteractionWalksOn(item);
                case WiredInteraction.WALKS_OFF_FURNI: return new WiredInteractionWalksOff(item);
                case WiredInteraction.STATE_CHANGED: return new WiredInteractionStateChanged(item);
                case WiredInteraction.SAY_SOMETHING: return new WiredInteractionSaysSomething(item);
                case WiredInteraction.GAME_STARTS: return new WiredInteractionGameStarts(item);
                case WiredInteraction.GAME_ENDS: return new WiredInteractionGameEnds(item);
                case WiredInteraction.REPEATER: return new WiredInteractionRepeater(item);
                case WiredInteraction.DEFAULT: default: return new WiredInteractionDefault(item);
            }
        }
    }

    public interface IWiredInteractor
    {
        IWiredData WiredData { get; set; }
        void OnTrigger(params object[] args);
        void OnCycle();
    }
}
