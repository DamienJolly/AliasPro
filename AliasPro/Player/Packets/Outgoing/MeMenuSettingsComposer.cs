namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models;

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
