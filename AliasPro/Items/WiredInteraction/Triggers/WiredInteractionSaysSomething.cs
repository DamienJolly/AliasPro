using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionSaysSomething : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.SAY_SOMETHING;

        public IWiredData WiredData { get; set; }

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

            if (entity is PlayerEntity userEntity)
            {
                if (OwnerOnly &&
                    !_item.CurrentRoom.Rights.IsOwner(userEntity.Player.Id)) return false;
            }

            if (_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile roomTile))
            {
                _item.CurrentRoom.Items.TriggerEffects(roomTile, entity);
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
