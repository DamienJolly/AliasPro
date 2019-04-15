﻿using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Items.Packets.Events
{
    public class RemoveItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RemoveItemMessageEvent;
        
        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            clientPacket.ReadInt(); //??
            uint itemId = (uint)clientPacket.ReadInt();

            IRoom room = session.CurrentRoom;

            if (!room.Rights.HasRights(session.Player.Id)) return;

            if (room.Items.TryGetItem(itemId, out IItem item))
            {
                if (item.ItemData.Type == "s")
                {
                    room.Mapping.RemoveItem(item);
                    await room.SendAsync(new RemoveFloorItemComposer(item));
                }
                else
                {
                    await room.SendAsync(new RemoveWallItemComposer(item));
                }

                item.RoomId = 0;
                item.CurrentRoom = null;

                if(session.Player.Inventory.TryAddItem(item))
                {
                    room.Items.RemoveItem(item.Id);

                    await session.SendPacketAsync(new AddPlayerItemsComposer(item));
                    await session.SendPacketAsync(new InventoryRefreshComposer());
                }
            }
        }
    }
}