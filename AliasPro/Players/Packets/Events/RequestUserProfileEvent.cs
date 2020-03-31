using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
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
    public class RequestUserProfileEvent : IMessageEvent
    {
        public short Header => Incoming.RequestUserProfileMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IGroupController _groupController;

        public RequestUserProfileEvent(
            IPlayerController playerController,
            IGroupController groupController)
        {
            _playerController = playerController;
            _groupController = groupController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int playerId = message.ReadInt();
            int friendCount;
            IPlayer targetPlayer = 
                await _playerController.GetPlayerAsync((uint)playerId);

            if (targetPlayer.Messenger != null)
            {
                friendCount = targetPlayer.Messenger.Friends.Count;
            }
            else
            {
                friendCount = await _playerController.GetPlayerFriendsAsync((uint)playerId);
            }

            IList<IGroup> groups = new List<IGroup>();
            foreach (int groupId in targetPlayer.Groups)
            {
                IGroup group = await _groupController.ReadGroupData(groupId);

                if (group != null)
                    groups.Add(group);
            }

            await session.SendPacketAsync(new UserProfileComposer(session.Player, targetPlayer, groups, friendCount));
        }
    }
}
