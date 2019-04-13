namespace AliasPro.API.Players.Models
{
    public interface IPlayerSettings
    {
        int NaviX { get; set; }
        int NaviY { get; set; }
        int NaviWidth { get; set; }
        int NaviHeight { get; set; }
        bool NaviHideSearches { get; set; }
        bool IgnoreInvites { get; set; }
        bool CameraFollow { get; set; }
        bool OldChat { get; set; }
        int VolumeSystem { get; set; }
        int VolumeFurni { get; set; }
        int VolumeTrax { get; set; }
    }
}
