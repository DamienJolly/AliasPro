using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class RequestProfileFriendsEvent : IMessageEvent
    {
        public short Header => Incoming.RequestProfileFriendsMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IMessengerController _messengerController;

        public RequestProfileFriendsEvent(
            IPlayerController playerController,
            IMessengerController messengerController)
        {
            _playerController = playerController;
            _messengerController = messengerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int playerId = message.ReadInt();

            ICollection<IMessengerFriend> targetFriends;

            if (_playerController.TryGetPlayer((uint)playerId, out IPlayer player) && player.Messenger != null)
            {
                targetFriends = player.Messenger.Friends;
            }
            else
            {
                IDictionary<uint, IMessengerFriend> targetFriendsDictionary = 
                    await _messengerController.GetPlayerFriendsAsync((uint)playerId);
                targetFriends = targetFriendsDictionary.Values;
            }

            await session.SendPacketAsync(new ProfileFriendsComposer(playerId, targetFriends));
        }
    }
}
