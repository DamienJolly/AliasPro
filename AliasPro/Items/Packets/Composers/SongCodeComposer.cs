using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class SongCodeComposer : IMessageComposer
    {
        private readonly ISongData _song;

        public SongCodeComposer(ISongData song)
        {
            _song = song;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.SongCodeMessageComposer);
            message.WriteString(_song.Name);
            message.WriteInt(_song.Id);
            return message;
        }
    }
}
