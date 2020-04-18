using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Items.Packets.Composers
{
    public class JukeBoxPlayListComposer : IMessageComposer
    {
        private readonly ICollection<IPlaylistSong> _songs;
        private readonly int _maxSongs;

        public JukeBoxPlayListComposer(ICollection<IPlaylistSong> songs, int maxSongs)
        {
            _songs = songs;
            _maxSongs = maxSongs;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.JukeBoxPlayListMessageComposer);
            message.WriteInt(_maxSongs);
            message.WriteInt(_songs.Count);
            foreach (IPlaylistSong song in _songs)
            {
                message.WriteInt(song.ItemId);
                message.WriteInt(song.SongData.Id);
            }
            return message;
        }
    }
}
