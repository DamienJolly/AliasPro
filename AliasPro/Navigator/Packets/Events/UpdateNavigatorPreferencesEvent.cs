using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Navigator.Packets.Events
{
    internal class UpdateNavigatorPreferencesEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UpdateNavigatorPreferencesMessageEvent;
        
        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            session.Player.PlayerSettings.NaviX = clientPacket.ReadInt();
            session.Player.PlayerSettings.NaviY = clientPacket.ReadInt();
            session.Player.PlayerSettings.NaviWidth = clientPacket.ReadInt();
            session.Player.PlayerSettings.NaviHeight = clientPacket.ReadInt();
            session.Player.PlayerSettings.NaviHideSearches = clientPacket.ReadBool();
            clientPacket.ReadInt();
        }
    }
}
