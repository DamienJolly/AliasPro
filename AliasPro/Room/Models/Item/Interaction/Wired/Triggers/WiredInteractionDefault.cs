using AliasPro.Item.Models;

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

        public void OnTrigger(params object[] args)
        {

        }

        public void OnCycle()
        {

        }
    }
}
