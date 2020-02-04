using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class UniqueIdEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.UniqueIdMessageEvent;

        public Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            clientPacket.ReadString();
            session.UniqueId = clientPacket.ReadString();
            return Task.CompletedTask;
        }
    }
}
