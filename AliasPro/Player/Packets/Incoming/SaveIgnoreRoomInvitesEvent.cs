using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;

    internal class SaveIgnoreRoomInvitesEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SaveIgnoreRoomInvitesMessageEvent;
        
        public Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            session.Player.PlayerSettings.IgnoreInvites = clientPacket.ReadBool();

            return Task.CompletedTask;
        }
    }
}
