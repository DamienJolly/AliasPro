namespace AliasPro.Room.Models.Item.Interaction
{
    using Network.Protocol;
    using AliasPro.Items.Models;
    using AliasPro.Room.Models.Entities;
    using AliasPro.Room.Packets.Composers;

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
            
            if (entity is UserEntity userEntity)
            {
                if (_item.ItemData.InteractionType == ItemInteraction.WIRED_TRIGGER)
                {
                    await userEntity.Session.SendPacketAsync(new WiredTriggerDataComposer(_item));
                }
                else if (_item.ItemData.InteractionType == ItemInteraction.WIRED_EFFECT)
                {
                    await userEntity.Session.SendPacketAsync(new WiredEffectDataComposer(_item));
                }
                else if (_item.ItemData.InteractionType == ItemInteraction.WIRED_CONDITION)
                {
                    await userEntity.Session.SendPacketAsync(new WiredConditionDataComposer(_item));
                }
            }
        }

        public void OnCycle()
        {
            if (_item.ItemData.WiredInteractionType == WiredInteraction.REPEATER ||
                _item.ItemData.WiredInteractionType == WiredInteraction.REPEATER_LONG)
            {
                _item.WiredInteraction.OnTrigger();
            }
             
            _item.WiredInteraction.OnCycle();
        }
    }
}
