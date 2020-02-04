using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Navigator.Packets.Composers
{
    public class NavigatorPreferencesComposer : IMessageComposer
    {
        private readonly IPlayerSettings _playerSettings;

        public NavigatorPreferencesComposer(IPlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.NavigatorPreferencesMessageComposer);
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
