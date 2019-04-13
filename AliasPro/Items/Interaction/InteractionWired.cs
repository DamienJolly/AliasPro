using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Types;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionWired : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionWired(IItem item)
        {
            _item = item;
        }

        public void Compose(ServerPacket message)
        {
            message.WriteInt(0);
            message.WriteString(_item.Mode.ToString());
        }

        public void OnUserWalkOn(BaseEntity entity)
        {

        }

        public void OnUserWalkOff(BaseEntity entity)
        {

        }

        public async void OnUserInteract(BaseEntity entity, int state)
        {
            if (entity == null) return;

            if (entity is PlayerEntity userEntity)
            {
                if (_item.ItemData.InteractionType == ItemInteractionType.WIRED_TRIGGER)
                {
                    await userEntity.Session.SendPacketAsync(new WiredTriggerDataComposer(_item));
                }
                else if (_item.ItemData.InteractionType == ItemInteractionType.WIRED_EFFECT)
                {
                    await userEntity.Session.SendPacketAsync(new WiredEffectDataComposer(_item));
                }
                else if (_item.ItemData.InteractionType == ItemInteractionType.WIRED_CONDITION)
                {
                    await userEntity.Session.SendPacketAsync(new WiredConditionDataComposer(_item));
                }
            }
        }

        public void OnCycle()
        {
            if (_item.ItemData.WiredInteractionType == WiredInteractionType.REPEATER ||
                _item.ItemData.WiredInteractionType == WiredInteractionType.REPEATER_LONG)
            {
                _item.WiredInteraction.OnTrigger();
            }

            _item.WiredInteraction.OnCycle();
        }
    }
}
