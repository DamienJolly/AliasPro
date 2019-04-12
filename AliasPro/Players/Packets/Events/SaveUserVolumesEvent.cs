using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Sessions;

namespace AliasPro.Players.Packets.Events
{
    internal class SaveUserVolumesEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SaveUserVolumesMessageEvent;
        
        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            session.Player.PlayerSettings.VolumeSystem = clientPacket.ReadInt();
            session.Player.PlayerSettings.VolumeFurni = clientPacket.ReadInt();
            session.Player.PlayerSettings.VolumeTrax = clientPacket.ReadInt();
        }
    }
}
