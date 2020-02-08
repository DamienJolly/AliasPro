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
            ClientMessage message)
        {
            session.Player.PlayerSettings.NaviX = message.ReadInt();
            session.Player.PlayerSettings.NaviY = message.ReadInt();
            session.Player.PlayerSettings.NaviWidth = message.ReadInt();
            session.Player.PlayerSettings.NaviHeight = message.ReadInt();
            session.Player.PlayerSettings.NaviHideSearches = message.ReadBoolean();
            message.ReadInt();

            return Task.CompletedTask;
        }
    }
}
