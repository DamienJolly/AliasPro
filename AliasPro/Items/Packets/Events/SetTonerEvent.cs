using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Interaction;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Items.Packets.Events
{
    public class SetTonerEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SetTonerMessageEvent;
        
        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) return;

            if (session.Entity == null) return;

            uint itemId = (uint)clientPacket.ReadInt();
            if (room.Items.TryGetItem(itemId, out IItem item))
            {
                if (item.Interaction is InteractionBackgroundToner interaction)
                {
                    int hue = interaction.Hue = clientPacket.ReadInt() % 256;
                    int saturation = interaction.Saturation = clientPacket.ReadInt() % 256;
                    int brightness = interaction.Brightness = clientPacket.ReadInt() % 256;

                    item.ExtraData = hue + ":" + saturation + ":" + brightness;
                    room.Items.TriggerWired(WiredInteractionType.STATE_CHANGED, session.Entity, item);
                    await room.SendAsync(new FloorItemUpdateComposer(item));
                }
            }
        }
    }
}
