﻿using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.Items.Interaction;
using AliasPro.Items.Types;

namespace AliasPro.Items.Utilities
{
    public class ItemInteractorUtility
    {
        public static IItemInteractor GetItemInteractor(ItemInteractionType interaction, IItem item)
        {
            switch (interaction)
            {
                case ItemInteractionType.GAME_TIMER: return new InteractionGameTimer(item);
                case ItemInteractionType.WIRED_TRIGGER: return new InteractionWired(item);
                case ItemInteractionType.WIRED_EFFECT: return new InteractionWired(item);
                case ItemInteractionType.WIRED_CONDITION: return new InteractionWired(item);
                case ItemInteractionType.VENDING_MACHINE: return new InteractionVendingMachine(item);
                case ItemInteractionType.ROLLER: return new InteractionRoller(item);
                case ItemInteractionType.DICE: return new InteractionDice(item);
                case ItemInteractionType.DEFAULT: default: return new InteractionDefault(item);
            }
        }
    }
}