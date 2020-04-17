using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Interaction;
using AliasPro.Items.Packets.Composers;
using AliasPro.Players.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class MannequinSaveLookEvent : IMessageEvent
    {
        public short Header => Incoming.MannequinSaveLookMessageEvent;
        
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

            IList<string> parts = new List<string>();
            foreach (string part in session.Player.Figure.Split('.'))
            {
                if (!part.Contains("hr") &&
                    !part.Contains("hd") &&
                    !part.Contains("he") &&
                    !part.Contains("ea") &&
                    !part.Contains("ha") &&
                    !part.Contains("fa"))
                    parts.Add(part);
            }

            string figure = string.Join(".", parts);

            interactionMannequin.Gender = session.Player.Gender == PlayerGender.MALE ? "m" : "f";
            interactionMannequin.Figure = figure;
            item.ExtraData = interactionMannequin.ExtraData;
            await room.SendPacketAsync(new FloorItemUpdateComposer(item));
        }
    }
}
