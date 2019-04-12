using AliasPro.Items.Models;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionDefault : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.DEFAULT;

        public WiredData WiredData { get; set; }

        public WiredInteractionDefault(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }

        public bool OnTrigger(params object[] args)
        {
            return true;
        }

        public void OnCycle()
        {

        }
    }
}
