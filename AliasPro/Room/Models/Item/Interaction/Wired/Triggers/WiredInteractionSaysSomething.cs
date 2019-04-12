using AliasPro.Items.Models;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionSaysSomething : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.SAY_SOMETHING;

        public WiredData WiredData { get; set; }

        public WiredInteractionSaysSomething(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }

        public bool OnTrigger(params object[] args)
        {
            BaseEntity entity = (BaseEntity)args[0];
            if (entity == null) return false;

            string text = (string)args[1];
            if (!text.Contains(" " + WiredData.Message) &&
                !text.Contains(WiredData.Message + " ") &&
                text != WiredData.Message)
                return false;

            if (entity is UserEntity userEntity)
            {
                if (OwnerOnly &&
                    !_item.CurrentRoom.RightHandler.IsOwner(userEntity.Player.Id)) return false;
            }

            if (_item.CurrentRoom.RoomMap.TryGetRoomTile(_item.Position.X, _item.Position.Y, out RoomTile roomTile))
            {
                _item.CurrentRoom.ItemHandler.TriggerEffects(roomTile, entity);
            }
            return true;
        }

        public void OnCycle()
        {

        }

        private bool OwnerOnly =>
            (WiredData.Params.Count != 1) ? false : WiredData.Params[0] == 1;
    }
}
