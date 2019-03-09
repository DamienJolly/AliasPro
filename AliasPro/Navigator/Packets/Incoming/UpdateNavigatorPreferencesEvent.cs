using System.Threading.Tasks;

namespace AliasPro.Navigator.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;

    internal class UpdateNavigatorPreferencesEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UpdateNavigatorPreferencesMessageEvent;
        
        public Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            session.Player.PlayerSettings.NaviX = clientPacket.ReadInt();
            session.Player.PlayerSettings.NaviY = clientPacket.ReadInt();
            session.Player.PlayerSettings.NaviWidth = clientPacket.ReadInt();
            session.Player.PlayerSettings.NaviHeight = clientPacket.ReadInt();
            session.Player.PlayerSettings.NaviHideSearches = clientPacket.ReadBool();
            clientPacket.ReadInt();

            return Task.CompletedTask;
        }
    }
}
