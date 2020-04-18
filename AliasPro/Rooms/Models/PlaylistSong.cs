using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;

namespace AliasPro.Rooms.Models
{
    internal class PlaylistSong : IPlaylistSong
    {
        public int ItemId { get; set; }
        public ISongData SongData { get; set; }

        internal PlaylistSong(int itemId, ISongData songData)
        {
            ItemId = itemId;
            SongData = songData;
        }
    }
}
