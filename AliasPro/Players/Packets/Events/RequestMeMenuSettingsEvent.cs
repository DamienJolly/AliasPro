using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class RequestMeMenuSettingsEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.RequestMeMenuSettingsMessageEvent;
        
        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            await session.SendPacketAsync(new MeMenuSettingsComposer(session.Player.PlayerSettings));
        }
    }
}
