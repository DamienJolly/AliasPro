namespace AliasPro.Player.Models
{
    public interface IPlayerSettings
    {
        int NaviX { get; set; }
        int NaviY { get; set; }
        int NaviWidth { get; set; }
        int NaviHeight { get; set; }
        bool NaviHideSearches { get; set; }
    }
}
