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

    public class WiredTriggerSaveDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.WiredTriggerSaveDataMessageEvent;
        
        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) return;

            if (!room.RightHandler.HasRights(session.Player.Id)) return;

            uint itemId = (uint)clientPacket.ReadInt();
            if (room.ItemHandler.TryGetItem(itemId, out IItem item))
            {
                item.WiredInteraction.SaveData(clientPacket);

                await session.SendPacketAsync(new WiredSavedComposer());
            }
        }
    }
}
