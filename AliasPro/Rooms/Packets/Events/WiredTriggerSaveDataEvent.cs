using AliasPro.API.Items.Models;
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
    public class WiredTriggerSaveDataEvent : IMessageEvent
    {
        public short Header => Incoming.WiredTriggerSaveDataMessageEvent;
        
        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) 
                return;

            if (!room.Rights.HasRights(session.Player.Id)) 
                return;

            uint wiredItemId = (uint)message.ReadInt();
            if (room.Items.TryGetItem(wiredItemId, out IItem wiredItem))
            {
                if (!wiredItem.ItemData.IsWired) 
                    return;

                IWiredData wiredData = wiredItem.WiredInteraction.WiredData;

                wiredData.Params.Clear();
                wiredData.Items.Clear();

                int paramCount = message.ReadInt();

                for (int i = 0; i < paramCount; i++)
                {
                    int paramData = message.ReadInt();
                    wiredData.Params.Add(paramData);
                }

                wiredData.Message = message.ReadString();

                int itemsCount = message.ReadInt();

                for (int i = 0; i < itemsCount; i++)
                {
                    int itemId = message.ReadInt();
                    if (room.Items.TryGetItem((uint)itemId, out IItem item))
                    {
                        wiredData.Items.Add(item.Id,
                            new WiredItemData(item.Id, item.Position, item.ExtraData, item.Rotation));
                    }
                }

                message.ReadInt();
                wiredItem.ExtraData = wiredData.ToString();

                await session.SendPacketAsync(new WiredSavedComposer());
            }
        }
    }
}
