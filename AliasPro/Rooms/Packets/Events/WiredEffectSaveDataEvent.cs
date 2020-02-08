﻿using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Models;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class WiredEffectSaveDataEvent : IMessageEvent
    {
        public short Header => Incoming.WiredEffectSaveDataMessageEvent;
        
        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) 
                return;
            
            if (!room.Rights.HasRights(session.Player.Id)) 
                return;

            uint wiredItemId = (uint)clientPacket.ReadInt();
            if (room.Items.TryGetItem(wiredItemId, out IItem wiredItem))
            {
                if (!wiredItem.ItemData.IsWired) 
                    return;

                IWiredData wiredData = wiredItem.WiredInteraction.WiredData;

                wiredData.Params.Clear();
                wiredData.Items.Clear();

                int paramCount = clientPacket.ReadInt();

                for (int i = 0; i < paramCount; i++)
                {
                    int paramData = clientPacket.ReadInt();
                    wiredData.Params.Add(paramData);
                }

                wiredData.Message = clientPacket.ReadString();

                int itemsCount = clientPacket.ReadInt();

                for (int i = 0; i < itemsCount; i++)
                {
                    int itemId = clientPacket.ReadInt();
                    if (room.Items.TryGetItem((uint)itemId, out IItem item))
                    {
                        wiredData.Items.Add(item.Id, 
                            new WiredItemData(item.Id, item.Position, item.Mode, item.Rotation));
                    }
                }

                wiredData.Delay = clientPacket.ReadInt();
                clientPacket.ReadInt();
                wiredItem.ExtraData = wiredData.ToString();

                await session.SendPacketAsync(new WiredSavedComposer());
            }
        }
    }
}
