using AliasPro.API.Items.Models;

namespace AliasPro.API.Rooms.Models
{
    public interface IPlaylistSong
    {
        int ItemId { get; set; }
        ISongData SongData { get; set; }
    }
}
