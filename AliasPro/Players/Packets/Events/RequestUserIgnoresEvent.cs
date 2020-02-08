using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class RequestUserIgnoresEvent : IMessageEvent
    {
        public short Header => Incoming.RequestUserIgnoresMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            await session.SendPacketAsync(new IgnoredUsersComposer(session.Player.Ignore.IgnoredUsers));
        }
    }
}
