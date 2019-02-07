namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Messenger;

    public class UpdateFriendComposer : IPacketComposer
    {
        private readonly IMessengerFriend _friend;

        public UpdateFriendComposer(IMessengerFriend friend)
        {
            _friend = friend;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UpdateFriendMessageComposer);
            message.WriteInt(0);
            message.WriteInt(1);
            message.WriteInt(0);
            _friend.Compose(message);
            return message;
        }
    }
}
