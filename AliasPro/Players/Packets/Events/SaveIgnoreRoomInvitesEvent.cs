using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    internal class SaveIgnoreRoomInvitesEvent : IMessageEvent
    {
        public short Header => Incoming.SaveIgnoreRoomInvitesMessageEvent;
        
        public Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            session.Player.PlayerSettings.IgnoreInvites = message.ReadBoolean();
            return Task.CompletedTask;
        }
    }
}
