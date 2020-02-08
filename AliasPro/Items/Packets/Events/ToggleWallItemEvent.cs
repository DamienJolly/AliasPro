using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class ToggleWallItemEvent : IMessageEvent
    {
        public short Header => Incoming.ToggleWallItemMessageEvent;
        
        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) 
                return;

            if (session.Entity == null) 
                return;
            
            if (!room.Rights.HasRights(session.Player.Id)) 
                return;

            uint itemId = (uint)clientPacket.ReadInt();
            if (room.Items.TryGetItem(itemId, out IItem item))
            {
                if (item.ItemData.Modes <= 1) 
                    return;

                if (item.ItemData.Type != "i") 
                    return;

                int state = clientPacket.ReadInt();
                
                item.Interaction.OnUserInteract(session.Entity, state);
                room.Items.TriggerWired(WiredInteractionType.STATE_CHANGED, session.Entity, item);

                await room.SendPacketAsync(new WallItemUpdateComposer(item));
            }
        }
    }
}
