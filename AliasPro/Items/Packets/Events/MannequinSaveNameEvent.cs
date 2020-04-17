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
    public class MannequinSaveNameEvent : IMessageEvent
    {
        public short Header => Incoming.MannequinSaveNameMessageEvent;
        
        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;

            if (room == null)
                return;

            if (room.OwnerId != session.Player.Id)
                return;

            int itemId = message.ReadInt();
            if (!room.Items.TryGetItem((uint)itemId, out IItem item))
                return;

            if (!(item.Interaction is InteractionMannequin interactionMannequin))
                return;

            string name = message.ReadString();

            interactionMannequin.OutfitName = name;
            item.ExtraData = interactionMannequin.ExtraData;
            await room.SendPacketAsync(new FloorItemUpdateComposer(item));
        }
    }
}
