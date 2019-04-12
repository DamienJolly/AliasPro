using AliasPro.Items.Models;
using AliasPro.Room.Gamemap;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionGameEnds : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.GAME_ENDS;

        public WiredData WiredData { get; set; }

        public WiredInteractionGameEnds(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }

        public bool OnTrigger(params object[] args)
        {
            if (_item.CurrentRoom.RoomMap.TryGetRoomTile(_item.Position.X, _item.Position.Y, out RoomTile roomTile))
            {
                _item.CurrentRoom.ItemHandler.TriggerEffects(roomTile, args);
            }
            return true;
        }

        public void OnCycle()
        {

        }
    }
}
