using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;

    internal class SaveUserVolumesEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SaveUserVolumesMessageEvent;
        
        public Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            session.Player.PlayerSettings.VolumeSystem = clientPacket.ReadInt();
            session.Player.PlayerSettings.VolumeFurni = clientPacket.ReadInt();
            session.Player.PlayerSettings.VolumeTrax = clientPacket.ReadInt();

            return Task.CompletedTask;
        }
    }
}
