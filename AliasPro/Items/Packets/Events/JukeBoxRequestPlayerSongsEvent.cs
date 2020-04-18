using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class JukeBoxRequestPlayerSongsEvent : IMessageEvent
    {
        public short Header => Incoming.JukeBoxRequestPlayerSongsMessageEvent;

		public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null)
                return;

            await session.SendPacketAsync(new JukeBoxMySongsComposer(room.Trax.AvailablePlayerSongs(session.Player.Inventory.Items)));
        }
    }
}
