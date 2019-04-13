using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Players.Packets.Composers
{
    public class MeMenuSettingsComposer : IPacketComposer
    {
        private readonly IPlayerSettings _settings;

        public MeMenuSettingsComposer(IPlayerSettings settings)
        {
            _settings = settings;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.MeMenuSettingsMessageComposer);
            message.WriteInt(_settings.VolumeSystem);
            message.WriteInt(_settings.VolumeFurni);
            message.WriteInt(_settings.VolumeTrax);
            message.WriteBoolean(_settings.OldChat);
            message.WriteBoolean(_settings.IgnoreInvites);
            message.WriteBoolean(_settings.CameraFollow);
            message.WriteInt(1); // dunno?
            message.WriteInt(0); // dunno?
            message.WriteInt(0); // dunno?
            return message;
        }
    }
}
