using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Players.Packets.Composers
{
    public class MeMenuSettingsComposer : IMessageComposer
    {
        private readonly IPlayerSettings _settings;

        public MeMenuSettingsComposer(IPlayerSettings settings)
        {
            _settings = settings;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.MeMenuSettingsMessageComposer);
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
