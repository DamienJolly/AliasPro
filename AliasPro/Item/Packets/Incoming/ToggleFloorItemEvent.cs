using System.Threading.Tasks;

namespace AliasPro.Item.Packets.Incoming
{
    using Models;
    using Room.Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Outgoing;

    public class ToggleFloorItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ToggleFloorItemMessageEvent;
        
        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) return;

            if (session.Entity == null) return;
            
            if (!room.RightHandler.HasRights(session.Player.Id)) return;

            uint itemId = (uint)clientPacket.ReadInt();
            if (room.ItemHandler.TryGetItem(itemId, out IItem item))
            {
                if (item.ItemData.Modes <= 1) return;

                if (item.ItemData.Type != "s") return;

                int state = clientPacket.ReadInt();
                
                item.Interaction.OnUserInteract(session.Entity, state);

                room.ItemHandler.TriggerWired(WiredInteraction.STATE_CHANGED, session.Entity, item.Id);
            }
        }
    }
}
