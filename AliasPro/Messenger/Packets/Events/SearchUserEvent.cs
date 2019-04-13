﻿using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Network.Events.Headers;
using System.Collections.Generic;

namespace AliasPro.Messenger.Packets.Events
{
    public class SearchUserEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SearchUserMessageEvent;

        private readonly IPlayerController _playerController;

        public SearchUserEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async void HandleAsync(
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
