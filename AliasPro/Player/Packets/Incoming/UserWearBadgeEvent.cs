using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Packets.Outgoing;
    using AliasPro.API.Player.Models;

    public class UserWearBadgeEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserWearBadgeMessageEvent;

        private readonly IPlayerController _playerController;

        public UserWearBadgeEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            if (session.Player.Badge == null) return;

            session.Player.Badge.ResetBadges();
            
            for (int i = 0; i < 5; i++)
            {
                int slot = clientPacket.ReadInt();
                string code = clientPacket.ReadString();

                if (slot < 1 || slot > 5 || code.Length == 0) continue;

                if (session.Player.Badge.TryGetBadge(code, out IPlayerBadge badge))
                {
                    badge.Slot = slot;
                }
            }

            if (session.CurrentRoom != null)
            {
                await session.CurrentRoom.SendAsync(new UserBadgesComposer(
                    session.Player.Badge.WornBadges, 
                    session.Player.Id));
                return;
            }

            await session.SendPacketAsync(new UserBadgesComposer(
                    session.Player.Badge.WornBadges,
                    session.Player.Id));
        }
    }
}
