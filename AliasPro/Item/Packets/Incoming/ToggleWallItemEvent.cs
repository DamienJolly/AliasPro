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

    public class ToggleWallItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ToggleWallItemMessageEvent;
        
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

                if (item.ItemData.Type != "i") return;

                int state = clientPacket.ReadInt();
                
                item.Interaction.OnUserInteract(session.Entity, state);

                await room.SendAsync(new WallItemUpdateComposer(item));
            }
        }
    }
}
