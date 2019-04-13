using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using System.Data.Common;

namespace AliasPro.Players.Models
{
    internal class PlayerSettings : IPlayerSettings
    {
        internal PlayerSettings(DbDataReader reader)
        {
            NaviX = reader.ReadData<int>("navi_x");
            NaviY = reader.ReadData<int>("navi_y");
            NaviWidth = reader.ReadData<int>("navi_width");
            NaviHeight = reader.ReadData<int>("navi_height");
            NaviHideSearches = reader.ReadData<bool>("navi_hide_searches");
            IgnoreInvites = reader.ReadData<bool>("ignore_invites");
            CameraFollow = reader.ReadData<bool>("camera_follow");
            OldChat = reader.ReadData<bool>("old_chat");
            VolumeSystem = reader.ReadData<int>("volume_system");
            VolumeFurni = reader.ReadData<int>("volume_furni");
            VolumeTrax = reader.ReadData<int>("volume_trax");
        }

        public int NaviX { get; set; }
        public int NaviY { get; set; }
        public int NaviWidth { get; set; }
        public int NaviHeight { get; set; }
        public bool NaviHideSearches { get; set; }
        public bool IgnoreInvites { get; set; }
        public bool CameraFollow { get; set; }
        public bool OldChat { get; set; }
        public int VolumeSystem { get; set; }
        public int VolumeFurni { get; set; }
        public int VolumeTrax { get; set; }
    }
}
