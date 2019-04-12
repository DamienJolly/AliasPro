using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;
using AliasPro.Sessions;

namespace AliasPro.Items.Packets.Events
{
    public class ToggleWallItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ToggleWallItemMessageEvent;
        
        public async void HandleAsync(
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

                if (item.ItemData.Type != "i") return;

                int state = clientPacket.ReadInt();
                
                item.Interaction.OnUserInteract(session.Entity, state);
                room.ItemHandler.TriggerWired(WiredInteraction.STATE_CHANGED, session.Entity, item);

                await room.SendAsync(new WallItemUpdateComposer(item));
            }
        }
    }
}
