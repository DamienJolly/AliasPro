using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

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
