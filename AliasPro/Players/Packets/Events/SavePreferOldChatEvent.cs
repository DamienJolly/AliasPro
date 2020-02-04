using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    internal class SavePreferOldChatEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.SavePreferOldChatMessageEvent;
        
        public Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            session.Player.PlayerSettings.OldChat = clientPacket.ReadBoolean();
            return Task.CompletedTask;
        }
    }
}
