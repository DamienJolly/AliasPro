using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Items.Packets.Composers
{
    public class SongDataComposer : IMessageComposer
    {
        private readonly ICollection<ISongData> _songs;

        public SongDataComposer(ICollection<ISongData> songs)
        {
            _songs = songs;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.SongDataMessageComposer);
            message.WriteInt(_songs.Count);
            foreach (ISongData song in _songs)
            {
                message.WriteInt(song.Id);
                message.WriteString(song.Code);
                message.WriteString(song.Name);
                message.WriteString(song.Track);
                message.WriteInt(song.Length * 1000);
                message.WriteString(song.Author);
            }
            return message;
        }
    }
}
