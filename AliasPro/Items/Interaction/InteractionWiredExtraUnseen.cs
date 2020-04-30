using AliasPro.API.Items.Models;
using System.Collections.Generic;

namespace AliasPro.Items.Interaction
{
    public class InteractionWiredExtraUnseen : InteractionWired
    {
        private readonly IList<int> seenEffects = new List<int>();

        public InteractionWiredExtraUnseen(IItem item)
            : base(item)
        {

        }

        public override void OnMoveItem()
        {
            seenEffects.Clear();
        }

        public override void OnPickupItem()
        {
            seenEffects.Clear();
        }

        public IItem GetUnseenEffect(IList<IItem> effects)
        {
            IList<IItem> unseenEffects = new List<IItem>();
            foreach (IItem effect in effects)
            {
                if (!seenEffects.Contains((int)effect.Id))
                    unseenEffects.Add(effect);
            }

            IItem effectToGo = null;
            if (unseenEffects.Count != 0)
            {
                effectToGo = unseenEffects[0];
            }
            else
            {
                seenEffects.Clear();

                if (effects.Count != 0)
                    effectToGo = effects[0];
            }

            if (effectToGo != null)
                seenEffects.Add((int)effectToGo.Id);

            return effectToGo;
        }
    }
}
