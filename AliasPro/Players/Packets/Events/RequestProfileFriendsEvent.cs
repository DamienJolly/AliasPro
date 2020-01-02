using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Events
{
    public class RequestProfileFriendsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestProfileFriendsMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IMessengerController _messengerController;

        public RequestProfileFriendsEvent(
            IPlayerController playerController,
            IMessengerController messengerController)
        {
            _playerController = playerController;
            _messengerController = messengerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int playerId = clientPacket.ReadInt();

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
