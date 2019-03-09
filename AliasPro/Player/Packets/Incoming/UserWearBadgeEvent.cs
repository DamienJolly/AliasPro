using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Packets.Outgoing;
    using Models.Badge;

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

            foreach (IBadgeData badge in session.Player.Badge.WearableBadges)
            {
                badge.Slot = 0;
            }
            await _playerController.ResetPlayerWearableBadgesAsync(session.Player.Id);

            for (int i = 0; i < 5; i++)
            {
                int slot = clientPacket.ReadInt();
                string code = clientPacket.ReadString();

                if (slot < 1 || slot > 5 || code.Length == 0) continue;

                if (session.Player.Badge.TryGetBadge(code, out IBadgeData badge))
                {
                    badge.Slot = slot;
                    await _playerController.UpdatePlayerWearableBadgeAsync(
                        session.Player.Id,
                        badge.Code,
                        badge.Slot);
                }
            }

            if (session.CurrentRoom != null)
            {
                await session.CurrentRoom.SendAsync(new UserBadgesComposer(
                    session.Player.Badge.WearableBadges, 
                    session.Player.Id));
                return;
            }

            await session.SendPacketAsync(new UserBadgesComposer(
                    session.Player.Badge.WearableBadges,
                    session.Player.Id));
        }
    }
}
