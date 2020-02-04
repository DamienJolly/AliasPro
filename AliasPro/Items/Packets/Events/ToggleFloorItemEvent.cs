using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Types;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class ToggleFloorItemEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.ToggleFloorItemMessageEvent;
        
        public Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (room == null)
                return Task.CompletedTask;

            if (session.Entity == null)
                return Task.CompletedTask;

            uint itemId = (uint)clientPacket.ReadInt();
            if (room.Items.TryGetItem(itemId, out IItem item))
            {
                if (item.ItemData.Type != "s")
                    return Task.CompletedTask;

                if (item.ItemData.InteractionType == ItemInteractionType.DICE)
                    return Task.CompletedTask;

                int state = clientPacket.ReadInt();

                item.Interaction.OnUserInteract(session.Entity, state);
                
                room.Items.TriggerWired(WiredInteractionType.STATE_CHANGED, session.Entity, item);
            }

            return Task.CompletedTask;
        }
    }
}
