using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Players.Packets.Events
{
    internal class SaveIgnoreRoomInvitesEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SaveIgnoreRoomInvitesMessageEvent;
        
        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            session.Player.PlayerSettings.IgnoreInvites = clientPacket.ReadBool();
        }
    }
}
