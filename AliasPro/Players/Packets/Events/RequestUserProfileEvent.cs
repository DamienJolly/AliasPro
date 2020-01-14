using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
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
    public class RequestUserProfileEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestUserProfileMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IGroupController _groupController;

        public RequestUserProfileEvent(
            IPlayerController playerController,
            IGroupController groupController)
        {
            _playerController = playerController;
            _groupController = groupController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int playerId = clientPacket.ReadInt();

            IPlayerData playerdata;
            int friendCount;

            if (_playerController.TryGetPlayer((uint)playerId, out IPlayer player))
            {
                playerdata = player;

                if (player.Messenger != null)
                    friendCount = player.Messenger.Friends.Count;
                else
                    friendCount = await _playerController.GetPlayerFriendsAsync((uint)playerId);
            }
            else
            {
                playerdata = await _playerController.GetPlayerDataAsync((uint)playerId);
                if (playerdata == null)
                    return;

                friendCount = await _playerController.GetPlayerFriendsAsync((uint)playerId);
            }

            IList<IGroup> groups = new List<IGroup>();
            foreach (int groupId in playerdata.Groups)
            {
                IGroup group = await _groupController.ReadGroupData(groupId);

                if (group != null)
                    groups.Add(group);
            }

            await session.SendPacketAsync(new UserProfileComposer(session.Player, playerdata, groups, friendCount));
        }
    }
}
