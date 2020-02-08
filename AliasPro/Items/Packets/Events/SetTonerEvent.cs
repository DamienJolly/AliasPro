using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Interaction;
using AliasPro.Items.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class SetTonerEvent : IMessageEvent
    {
        public short Header => Incoming.SetTonerMessageEvent;
        
        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) 
                return;

            if (session.Entity == null) 
                return;

            uint itemId = (uint)message.ReadInt();
            if (room.Items.TryGetItem(itemId, out IItem item))
            {
                if (item.Interaction is InteractionBackgroundToner interaction)
                {
                    int hue = interaction.Hue = message.ReadInt() % 256;
                    int saturation = interaction.Saturation = message.ReadInt() % 256;
                    int brightness = interaction.Brightness = message.ReadInt() % 256;

                    item.ExtraData = hue + ":" + saturation + ":" + brightness;
                    await room.SendPacketAsync(new FloorItemUpdateComposer(item));
                }
            }
        }
    }
}
