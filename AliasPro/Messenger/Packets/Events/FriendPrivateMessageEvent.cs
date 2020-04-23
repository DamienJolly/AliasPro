using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Messenger.Models;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Players.Types;
using AliasPro.Utilities;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Events
{
    public class FriendPrivateMessageEvent : IMessageEvent
    {
        public short Header => Incoming.FriendPrivateMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IMessengerController _messengerController;

        public FriendPrivateMessageEvent(IPlayerController playerController, IMessengerController messengerController)
        {
            _playerController = playerController;
            _messengerController = messengerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            uint targetId = (uint)message.ReadInt();

            if (!_playerController.TryGetPlayer(targetId, out IPlayer targetPlayer))
                return;

            string msg = message.ReadString();
            if (string.IsNullOrWhiteSpace(msg))
                return;

            if (msg.Length > 200)
                msg.Substring(0, 200);

            if (!session.Player.Messenger.TryGetFriend(targetPlayer.Id, out IMessengerFriend friend))
            {
                await session.SendPacketAsync(new RoomInviteErrorComposer(RoomInviteErrorComposer.NO_FRIENDS, targetPlayer.Id));
                return;
            }

            if (session.Player.Sanction.GetCurrentSanction(out IPlayerSanction playerSanction) && playerSanction.Type == SanctionType.MUTE)
            {
                await session.SendPacketAsync(new RoomInviteErrorComposer(RoomInviteErrorComposer.YOU_ARE_MUTED, targetPlayer.Id));
                return;
            }

            IMessengerMessage privateMessage =
                    new MessengerMessage(session.Player.Id, msg, (int)UnixTimestamp.Now);
            //todo: log message

            if (targetPlayer.Session != null)
            {
                if (targetPlayer.Sanction.GetCurrentSanction(out IPlayerSanction targetSanction) && targetSanction.Type == SanctionType.MUTE)
                {
                    await session.SendPacketAsync(new RoomInviteErrorComposer(RoomInviteErrorComposer.FRIEND_MUTED, targetPlayer.Id));
                    return;
                }

                await targetPlayer.Session.SendPacketAsync(new FriendChatComposer(privateMessage));
            }
            else
            {
                await _messengerController.AddOfflineMessageAsync(targetPlayer.Id, privateMessage);
            }
        }
    }
}
