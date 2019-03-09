using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;

    internal class SavePreferOldChatEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SavePreferOldChatMessageEvent;
        
        public Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            session.Player.PlayerSettings.OldChat = clientPacket.ReadBool();

            return Task.CompletedTask;
        }
    }
}
