using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Sessions;

namespace AliasPro.Players.Packets.Events
{
    internal class SavePreferOldChatEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SavePreferOldChatMessageEvent;
        
        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            session.Player.PlayerSettings.OldChat = clientPacket.ReadBool();
        }
    }
}
