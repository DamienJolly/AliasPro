using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Messenger.Models;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Utilities;

namespace AliasPro.Messenger.Packets.Events
{
    public class FriendPrivateMessageEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.FriendPrivateMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IMessengerController _messengerController;

        public FriendPrivateMessageEvent(IPlayerController playerController, IMessengerController messengerController)
        {
            _playerController = playerController;
            _messengerController = messengerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint targetId = (uint)clientPacket.ReadInt();

            if (!_playerController.TryGetPlayer(targetId, out IPlayer targetPlayer))
                return;

            string message = clientPacket.ReadString();
            if (string.IsNullOrWhiteSpace(message))
                return;

            if (message.Length > 200)
                message.Substring(0, 200);

            if (!session.Player.Messenger.TryGetFriend(targetPlayer.Id, out IMessengerFriend friend))
            {
                await session.SendPacketAsync(new RoomInviteErrorComposer(RoomInviteErrorComposer.NO_FRIENDS, targetPlayer.Id));
                return;
            }

            //todo: muted
            /*if ()
            {
                await session.SendPacketAsync(new RoomInviteErrorComposer(RoomInviteErrorComposer.YOU_ARE_MUTED, targetPlayer.Id));
                return;
            }*/

            IMessengerMessage privateMessage =
                    new MessengerMessage(session.Player.Id, message, (int)UnixTimestamp.Now);
            //todo: log message

            if (targetPlayer.Session != null)
            {
                //todo: muted
                /*if ()
                {
                    await session.SendPacketAsync(new RoomInviteErrorComposer(RoomInviteErrorComposer.FRIEND_MUTED, targetPlayer.Id));
                    return;
                }*/

                await targetPlayer.Session.SendPacketAsync(new FriendChatComposer(privateMessage));
            }
            else
            {
                await _messengerController.AddOfflineMessageAsync(targetPlayer.Id, privateMessage);
            }
        }
    }
}
