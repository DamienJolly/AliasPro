using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class UserWearBadgeEvent : IMessageEvent
    {
        public short Header => Incoming.UserWearBadgeMessageEvent;

        private readonly IPlayerController _playerController;

        public UserWearBadgeEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
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
                await session.CurrentRoom.SendPacketAsync(new UserBadgesComposer(
                    session.Player.Badge.WornBadges, 
                    (int)session.Player.Id));
                return;
            }

            await session.SendPacketAsync(new UserBadgesComposer(
                    session.Player.Badge.WornBadges,
                    (int)session.Player.Id));
        }
    }
}
