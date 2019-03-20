using AliasPro.Item.Models;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionSaysSomething : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.SAY_COMMAND;

        public IWiredData WiredData { get; set; }

        public WiredInteractionSaysSomething(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }

        public void OnTrigger(params object[] args)
        {
            BaseEntity entity = (BaseEntity)args[0];
            if (entity == null) return;

            string text = (string)args[1];
            if (!text.Contains(" " + WiredData.Message) &&
                !text.Contains(WiredData.Message + " ") &&
                text != WiredData.Message)
                return;

            if (entity is UserEntity userEntity)
            {
                if (OwnerOnly &&
                    !_item.CurrentRoom.RightHandler.IsOwner(userEntity.Player.Id)) return;
            }

            foreach (IItem effect in _item.CurrentRoom.RoomMap.GetRoomTile(_item.Position.X, _item.Position.Y).WiredEffects)
            {
                effect.WiredInteraction.OnTrigger(entity);
            }
        }

        public void OnCycle()
        {

        }

        private bool OwnerOnly =>
            (WiredData.Params.Count != 1) ? false : WiredData.Params[0] == 1;
    }
}
