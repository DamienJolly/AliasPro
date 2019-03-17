﻿namespace AliasPro.Room.Models.Item.Interaction
{
    using Network.Protocol;
    using Packets.Outgoing;
    using AliasPro.Item.Models;
    using Sessions;
    using AliasPro.Room.Models.Entities;

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
            if (_item.ItemData.WiredInteractionType == WiredInteraction.WALKS_ON_FURNI)
            {
                _item.WiredInteraction.OnTrigger(entity);
            }
        }

        public void OnUserWalkOff(BaseEntity entity)
        {
            if (_item.ItemData.WiredInteractionType == WiredInteraction.WALKS_OFF_FURNI)
            {
                _item.WiredInteraction.OnTrigger(entity);
            }
        }

        public async void OnUserInteract(BaseEntity entity, int state)
        {
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
            }
        }

        public void OnCycle()
        {
            if (_item.ItemData.WiredInteractionType == WiredInteraction.REPEATER)
            {
                _item.WiredInteraction.OnTrigger(null);
            }
             
            _item.WiredInteraction.OnCycle();
        }
    }
}