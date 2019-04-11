using System.Threading.Tasks;
using System.Collections.Generic;
using AliasPro.Network.Protocol;
using AliasPro.Sessions;
using AliasPro.Network.Events;
using AliasPro.API.Player.Models;
using AliasPro.API.Messenger.Models;
using AliasPro.Messenger.Packets.Outgoing;
using AliasPro.Player;

namespace AliasPro.Messenger.Packets.Incoming
{
    using Network.Events.Headers;

    public class SearchUserEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SearchUserMessageEvent;

        private readonly IPlayerController _playerController;

        public SearchUserEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string username = clientPacket.ReadString();

            if (string.IsNullOrEmpty(username))
                return;

            ICollection<IPlayer> friends = new List<IPlayer>();
            ICollection<IPlayer> notFriends = new List<IPlayer>();
            IDictionary<uint, IPlayer> players = new Dictionary<uint, IPlayer>();
                //todo: await _playerController.GetPlayersByUsernameAsync(username);

            foreach (IPlayer player in players.Values)
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
