using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Types;
using AliasPro.Rooms.Games.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionGiveScore : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.GIVE_SCORE;

        public WiredInteractionGiveScore(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            if (args.Length == 0)
                return false;

            BaseEntity target = (BaseEntity)args[0];

            if (target.GamePlayer == null || target.GamePlayer.Game.State != GameState.RUNNING)
                return false;

            if (target.GamePlayer.Game.TimesGivenScore >= MaxPoints)
                return true;

            target.GamePlayer.Game.TimesGivenScore++;
            target.GamePlayer.Points += Points;
            Room.Items.TriggerWired(WiredInteractionType.SCORE_ACHIEVED, target.GamePlayer.Team.TotalPoints);
            return true;
        }

        private int Points =>
            (WiredData.Params.Count <= 0) ? 1 : WiredData.Params[0];

        private int MaxPoints =>
            (WiredData.Params.Count <= 1) ? 1 : WiredData.Params[1];
    }
}
