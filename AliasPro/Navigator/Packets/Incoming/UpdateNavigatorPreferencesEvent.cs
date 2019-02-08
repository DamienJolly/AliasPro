using System.Threading.Tasks;

namespace AliasPro.Navigator.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Player;
    using Player.Models;

    internal class UpdateNavigatorPreferencesEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UpdateNavigatorPreferencesMessageEvent;

        private readonly IPlayerController _playerController;

        public UpdateNavigatorPreferencesEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IPlayerSettings settings = 
                session.Player.PlayerSettings;
            settings.NaviX = clientPacket.ReadInt();
            settings.NaviY = clientPacket.ReadInt();
            settings.NaviWidth = clientPacket.ReadInt();
            settings.NaviHeight = clientPacket.ReadInt();
            settings.NaviHideSearches = clientPacket.ReadBool();
            clientPacket.ReadInt();
            return Task.CompletedTask;
        }
    }
}
