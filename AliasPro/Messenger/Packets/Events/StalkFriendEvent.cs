using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Messenger.Packets.Events
{
    public class StalkFriendEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.StalkFriendMessageEvent;

        private readonly IPlayerController _playerController;

        public StalkFriendEvent(
            IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint targetId = (uint)clientPacket.ReadInt();

            if (!_playerController.TryGetPlayer(targetId, out IPlayer targetPlayer) || 
                targetPlayer.Session == null)
            {
                await session.SendPacketAsync(new StalkErrorComposer(StalkErrorComposer.FRIEND_OFFLINE));
                return;
            }

            if (!session.Player.Messenger.TryGetFriend(targetPlayer.Id, out IMessengerFriend friend))
            {
                await session.SendPacketAsync(new StalkErrorComposer(StalkErrorComposer.NOT_IN_FRIEND_LIST));
                return;
            }

            if (!friend.InRoom)
            {
                await session.SendPacketAsync(new StalkErrorComposer(StalkErrorComposer.FRIEND_NOT_IN_ROOM));
                return;
            }

            if (session.CurrentRoom == targetPlayer.Session.CurrentRoom)
                return;

            await session.SendPacketAsync(new ForwardToRoomComposer(targetPlayer.Session.CurrentRoom.Id));
        }
    }
}
