using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Utilities;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Models;
    using Packets.Outgoing;
    using Models.Messenger;

    public class FriendPrivateMessageEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.FriendPrivateMessageEvent;

        private readonly IPlayerController _playerController;

        public FriendPrivateMessageEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint targetId = (uint)clientPacket.ReadInt();
            IPlayer targetPlayer =
                await _playerController.GetPlayerByIdAsync(targetId);

            if (targetPlayer == null)
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
                await _playerController.AddOfflineMessageAsync(targetPlayer.Id, privateMessage);
            }
        }
    }
}
