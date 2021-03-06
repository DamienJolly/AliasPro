﻿using AliasPro.API.Items.Models;
using AliasPro.Items.Interaction;
using AliasPro.Items.Types;

namespace AliasPro.Items.Utilities
{
    public class ItemInteractorUtility
    {
        public static ItemInteraction GetItemInteraction(ItemInteractionType interaction, IItem item)
        {
            switch (interaction)
            {
                case ItemInteractionType.DEFAULT: default: return new InteractionDefault(item);
                case ItemInteractionType.GAME_TIMER: return new InteractionGameTimer(item);
                case ItemInteractionType.VENDING_MACHINE: return new InteractionVendingMachine(item);
                case ItemInteractionType.DICE: return new InteractionDice(item);
				case ItemInteractionType.EXCHANGE: return new InteractionExchange(item);
				case ItemInteractionType.LOVE_LOCK: return new InteractionLoveLock(item);
				case ItemInteractionType.BADGE_DISPLAY: return new InteractionBadgeDisplay(item);
				case ItemInteractionType.ONE_WAY_GATE: return new InteractionOneWayGate(item);
				case ItemInteractionType.CLOTHING: return new InteractionClothing(item);
				case ItemInteractionType.BOTTLE: return new InteractionSpinningBottle(item);
				case ItemInteractionType.TROPHY: return new InteractionTrophy(item);
				case ItemInteractionType.TELEPORT: return new InteractionTeleport(item);
				case ItemInteractionType.GIFT: return new InteractionGift(item);
				case ItemInteractionType.PRESSURE_TILE: return new InteractionPressureTile(item);
				case ItemInteractionType.TENT: return new InteractionTent(item);
				case ItemInteractionType.WATER: return new InteractionWater(item);
				case ItemInteractionType.BACKGROUND_TONER: return new InteractionBackgroundToner(item);
				case ItemInteractionType.ECOTRON: return new InteractionEcotron(item);
				case ItemInteractionType.DIMMER: return new InteractionDimmer(item);
				case ItemInteractionType.MANNEQUIN: return new InteractionMannequin(item);
				case ItemInteractionType.CRACKABLE: return new InteractionCrackable(item);
				case ItemInteractionType.MUSICDISC: return new InteractionMusicDisc(item);
				case ItemInteractionType.JUKEBOX: return new InteractionJukeBox(item);
				case ItemInteractionType.BALL: return new InteractionBall(item);
				case ItemInteractionType.FOOTBALL_SCOREBOARD: return new InteractionScoreboard(item);

				//wired
				case ItemInteractionType.WIRED_TRIGGER: return new InteractionWiredTrigger(item);
				case ItemInteractionType.WIRED_EFFECT: return new InteractionWiredEffect(item);
				case ItemInteractionType.WIRED_CONDITION: return new InteractionWiredCondition(item);

				case ItemInteractionType.WIRED_BLOB: return new InteractionWiredBlob(item);
				case ItemInteractionType.WIRED_PYRAMID: return new InteractionWiredPyramid(item);

				case ItemInteractionType.WIRED_EXTRA_RANDOM: return new InteractionWiredExtraRandom(item);
				case ItemInteractionType.WIRED_EXTRA_UNSEEN: return new InteractionWiredExtraUnseen(item);
			}
		}
    }
}