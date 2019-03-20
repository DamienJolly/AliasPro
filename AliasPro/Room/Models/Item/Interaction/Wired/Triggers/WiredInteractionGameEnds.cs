using AliasPro.Item.Models;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionGameEnds : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.GAME_ENDS;

        public IWiredData WiredData { get; set; }

        public WiredInteractionGameEnds(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }

        public void OnTrigger(params object[] args)
        {
            foreach (IItem effect in _item.CurrentRoom.RoomMap.GetRoomTile(_item.Position.X, _item.Position.Y).WiredEffects)
            {
                effect.WiredInteraction.OnTrigger(args);
            }
        }

        public void OnCycle()
        {

        }
    }
}
