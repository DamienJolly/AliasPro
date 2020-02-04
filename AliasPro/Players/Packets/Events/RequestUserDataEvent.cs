using AliasPro.API.Players;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class RequestUserDataEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.RequestUserDataMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestUserDataEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            await session.SendPacketAsync(new UserDataComposer(session.Player));
            await session.SendPacketAsync(new UserPerksComposer(session.Player));
        }
    }
}
