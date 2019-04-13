using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Players.Packets.Events
{
    internal class SaveBlockCameraFollowEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SaveBlockCameraFollowMessageEvent;
        
        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            session.Player.PlayerSettings.CameraFollow = clientPacket.ReadBool();
        }
    }
}
