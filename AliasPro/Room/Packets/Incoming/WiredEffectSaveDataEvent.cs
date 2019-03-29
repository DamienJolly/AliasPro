using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Models;
    using AliasPro.Item.Models;
    using Outgoing;
    using AliasPro.Room.Models.Item.Interaction.Wired;

    public class WiredEffectSaveDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.WiredEffectSaveDataMessageEvent;
        
        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) return;

            if (!room.RightHandler.HasRights(session.Player.Id)) return;

            uint wiredItemId = (uint)clientPacket.ReadInt();
            if (room.ItemHandler.TryGetItem(wiredItemId, out IItem wiredItem))
            {
                if (!wiredItem.ItemData.IsWired) return;

                WiredData wiredData = wiredItem.WiredInteraction.WiredData;

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
                    if (room.ItemHandler.TryGetItem((uint)itemId, out IItem item))
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
