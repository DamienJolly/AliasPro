using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;

    internal class SaveBlockCameraFollowEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SaveBlockCameraFollowMessageEvent;
        
        public Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            session.Player.PlayerSettings.CameraFollow = clientPacket.ReadBool();

            return Task.CompletedTask;
        }
    }
}
