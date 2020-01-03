﻿using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Interaction
{
    public class InteractionFloor : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionFloor(IItem item)
        {
            _item = item;
        }

        public void Compose(ServerPacket message, bool tradeItem)
        {
            if (!tradeItem)
                message.WriteInt(3);
            message.WriteInt(0);
            message.WriteString(_item.ExtraData);
        }

        public void OnPlaceItem()
        {

        }

        public void OnPickupItem()
        {

        }

        public void OnMoveItem()
        {

        }

        public void OnUserWalkOn(BaseEntity entity)
        {

        }

        public void OnUserWalkOff(BaseEntity entity)
        {

        }

        public void OnUserInteract(BaseEntity entity, int state)
        {

        }

        public void OnCycle()
        {

        }
    }
}