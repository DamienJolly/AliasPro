using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;

namespace AliasPro.Items.Interaction.Wired
{
    public abstract class WiredInteraction
    {
        public IItem Item;

        public IRoom Room =>
            Item.CurrentRoom;
        public IWiredData WiredData { get; set; }

        public WiredInteraction(IItem item, int type)
        {
            Item = item;
            WiredData =
                new WiredData(type, Item.WiredData);
        }

        public abstract bool Execute(params object[] args);

        public virtual void OnCycle() { }
    }
}
