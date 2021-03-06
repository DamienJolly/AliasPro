﻿using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction
{
    public class InteractionDefault : ItemInteraction
    {
        public InteractionDefault(IItem item)
            : base(item)
        {
            
        }

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(0);
            message.WriteString(Item.ExtraData);
        }
        
        public async override void OnUserInteract(BaseEntity entity, int state)
        {
            int modes = Item.ItemData.Modes - 1;
            if (modes <= 0)
                return;

            if (entity is PlayerEntity playerEntity)
				if (!Room.Rights.HasRights(playerEntity.Player.Id)) return;


            if (Item.ItemData.Modes > 0)
            {
                if (!int.TryParse(Item.ExtraData, out int currentState))
                {
                    currentState = 0;
                }

                Item.ExtraData = "" + (currentState + 1) % Item.ItemData.Modes;
            }

			await Room.SendPacketAsync(new FloorItemUpdateComposer(Item));
        }
    }
}
