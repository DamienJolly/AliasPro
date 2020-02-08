using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Packets.Events
{
    internal class UpdateNavigatorPreferencesEvent : IMessageEvent
    {
        public short Header => Incoming.UpdateNavigatorPreferencesMessageEvent;
        
        public Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            session.Player.PlayerSettings.NaviX = clientPacket.ReadInt();
            session.Player.PlayerSettings.NaviY = clientPacket.ReadInt();
            session.Player.PlayerSettings.NaviWidth = clientPacket.ReadInt();
            session.Player.PlayerSettings.NaviHeight = clientPacket.ReadInt();
            session.Player.PlayerSettings.NaviHideSearches = clientPacket.ReadBoolean();
            clientPacket.ReadInt();

            return Task.CompletedTask;
        }
    }
}
