using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class JukeBoxRequestCurrentSongEvent : IMessageEvent
    {
        public short Header => Incoming.JukeBoxRequestCurrentSongMessageEvent;

        public Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null)
                return Task.CompletedTask;

            if (room.Trax != null)
                room.Trax.LoadCurrentSong(session);

            return Task.CompletedTask;
        }
    }
}
