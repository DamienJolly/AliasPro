﻿using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Protocol;
using AliasPro.Players.Packets.Composers;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction
{
    public class InteractionExchange: IItemInteractor
    {
        private readonly IItem _item;

        public InteractionExchange(IItem item)
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
			if (!int.TryParse(_item.ItemData.ExtraData, out int amount))
			{
				return;
			}

			if (entity is PlayerEntity userEntity)
			{
				userEntity.Player.Credits += amount;
				await userEntity.Player.Session.SendPacketAsync(new UserCreditsComposer(userEntity.Player.Credits));
			}
        }

        public void OnCycle()
        {

        }
    }
}