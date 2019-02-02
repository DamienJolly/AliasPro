namespace AliasPro.Navigator.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Player.Models;

    public class NavigatorPreferencesComposer : IPacketComposer
    {
        private readonly IPlayerSettings _playerSettings;

        public NavigatorPreferencesComposer(IPlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.NavigatorPreferencesMessageComposer);
            message.WriteInt(_playerSettings.NaviX);
            message.WriteInt(_playerSettings.NaviY);
            message.WriteInt(_playerSettings.NaviWidth);
            message.WriteInt(_playerSettings.NaviHeight);
            message.WriteBoolean(_playerSettings.NaviHideSearches);
            message.WriteInt(0); //dunno??
            return message;
        }
    }
}
