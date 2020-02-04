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
    public class ToggleOneWayEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.ToggleOneWayMessageEvent;
        
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
                if (item.ItemData.InteractionType != ItemInteractionType.ONE_WAY_GATE)
                    return Task.CompletedTask;

                item.Interaction.OnUserInteract(session.Entity, 1);
                room.Items.TriggerWired(WiredInteractionType.STATE_CHANGED, session.Entity, item);
            }

            return Task.CompletedTask;
        }
    }
}
