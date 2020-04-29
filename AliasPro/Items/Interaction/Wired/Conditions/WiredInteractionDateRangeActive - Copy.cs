using AliasPro.API.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionDateRangeActive : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.DATE_RANGE;

        public WiredInteractionDateRangeActive(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            //todo: code 
            return true;
        }
    }
}
