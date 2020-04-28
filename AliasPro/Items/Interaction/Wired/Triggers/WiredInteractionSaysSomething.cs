using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionSaysSomething : WiredInteraction
    {
        private static readonly WiredTriggerType _type = WiredTriggerType.SAY_SOMETHING;

        public WiredInteractionSaysSomething(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
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
                    !Room.Rights.IsOwner(userEntity.Player.Id)) return false;
            }

            if (Room.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile roomTile))
            {
                Room.Items.TriggerEffects(roomTile, entity);
            }
            return true;
        }

        private bool OwnerOnly =>
            (WiredData.Params.Count != 1) ? false : WiredData.Params[0] == 1;
    }
}
