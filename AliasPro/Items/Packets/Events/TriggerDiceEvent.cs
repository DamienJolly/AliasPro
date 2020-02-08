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
    public class ToggleDiceEvent : IMessageEvent
    {
        public short Header => Incoming.TriggerDiceMessageEvent;
        
        public Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;

            if (room == null)
                return Task.CompletedTask;

            if (session.Entity == null)
                return Task.CompletedTask;

            uint itemId = (uint)message.ReadInt();
            if (room.Items.TryGetItem(itemId, out IItem item))
            {
                if (item.ItemData.InteractionType != ItemInteractionType.DICE)
                    return Task.CompletedTask;

                item.Interaction.OnUserInteract(session.Entity, -1);
                room.Items.TriggerWired(WiredInteractionType.STATE_CHANGED, session.Entity, item);
            }

            return Task.CompletedTask;
        }
    }
}
