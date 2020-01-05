using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;

namespace AliasPro.Players.Packets.Events
{
    public class RequestUserIgnoresEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestUserIgnoresMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new IgnoredUsersComposer(session.Player.Ignore.IgnoredUsers));
        }
    }
}
