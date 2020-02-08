using AliasPro.API.Messenger.Models;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Events
{
    public class StalkFriendEvent : IMessageEvent
    {
        public short Header => Incoming.StalkFriendMessageEvent;

        private readonly IPlayerController _playerController;

        public StalkFriendEvent(
            IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            uint targetId = (uint)message.ReadInt();

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
