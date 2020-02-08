using AliasPro.API.Messenger.Models;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Messenger.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Events
{
    public class SearchUserEvent : IMessageEvent
    {
        public short Header => Incoming.SearchUserMessageEvent;

        private readonly IPlayerController _playerController;

        public SearchUserEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            string username = message.ReadString();

            if (string.IsNullOrEmpty(username))
                return;

            ICollection<IPlayerData> friends = new List<IPlayerData>();
            ICollection<IPlayerData> notFriends = new List<IPlayerData>();
            IDictionary<uint, IPlayerData> players = 
				await _playerController.GetPlayersByUsernameAsync(username);

            foreach (IPlayerData player in players.Values)
            {
                if (!session.Player.Messenger.TryGetFriend(player.Id, out IMessengerFriend friend))
                {
                    notFriends.Add(player);
                    continue;
                }
                friends.Add(player);
            }

            await session.SendPacketAsync(new UserSearchResultComposer(friends, notFriends));
        }
    }
}
