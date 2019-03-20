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
            if (room.ItemHandler.TryGetItem(wiredItemId, out IItem item))
            {
                if (!item.ItemData.IsWired) return;

                IWiredData wiredData = item.WiredInteraction.WiredData;

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
                    int itemId = clientPacket.ReadInt(); //todo: check if item exists
                    wiredData.Items.Add((uint)itemId);
                }

                wiredData.Delay = clientPacket.ReadInt();
                clientPacket.ReadInt();
                item.ExtraData = wiredData.DataToString;

                await session.SendPacketAsync(new WiredSavedComposer());
            }
        }
    }
}
