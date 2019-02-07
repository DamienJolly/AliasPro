using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Packets.Outgoing;
    using Models;
    using Models.Messenger;

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
            IDictionary<uint, IPlayer> players =
                await _playerController.GetPlayersByUsernameAsync(username);

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
