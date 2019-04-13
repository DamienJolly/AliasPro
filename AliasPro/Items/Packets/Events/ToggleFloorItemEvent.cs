using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Types;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;

namespace AliasPro.Items.Packets.Events
{
    public class ToggleFloorItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ToggleFloorItemMessageEvent;
        
        public void HandleAsync(
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
                if (item.ItemData.Type != "s") return;

                int state = clientPacket.ReadInt();
                
                item.Interaction.OnUserInteract(session.Entity, state);
                
                room.ItemHandler.TriggerWired(WiredInteractionType.STATE_CHANGED, session.Entity, item);
            }
        }
    }
}
