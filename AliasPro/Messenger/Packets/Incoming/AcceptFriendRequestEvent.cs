﻿using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Player.Models;
using AliasPro.Messenger.Models;
using AliasPro.Messenger.Packets.Outgoing;
using AliasPro.Network.Events;
using AliasPro.Network.Protocol;
using AliasPro.Player;
using AliasPro.Sessions;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Incoming
{
    using Network.Events.Headers;

    public class AcceptFriendRequestEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.AcceptFriendRequestMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IMessengerController _messengerController;

        public AcceptFriendRequestEvent(IPlayerController playerController, IMessengerController messengerController)
        {
            _playerController = playerController;
            _messengerController = messengerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int amount = clientPacket.ReadInt();
            for (int i = 0; i < amount; i++)
            {
                uint targetId = (uint)clientPacket.ReadInt();

                if (!_playerController.TryGetPlayer(targetId, out IPlayer targetPlayer))
                    continue;

                if (!session.Player.Messenger.TryGetRequest(targetPlayer.Id, out IMessengerRequest request))
                    continue;

                session.Player.Messenger.RemoveRequest(targetPlayer.Id);
                await _messengerController.RemoveRequestAsync(session.Player.Id, targetPlayer.Id);

                if (session.Player.Messenger.TryGetFriend(targetPlayer.Id, out IMessengerFriend friend))
                    continue;

                IMessengerFriend friendOne = new MessengerFriend(targetPlayer);
                if (session.Player.Messenger.TryAddFriend(friendOne))
                {
                    await session.SendPacketAsync(new UpdateFriendComposer(friendOne));
                }

                if (targetPlayer.Session != null && targetPlayer.Messenger != null)
                {
                    IMessengerFriend friendTwo = new MessengerFriend(session.Player);
                    if (targetPlayer.Session.Player.Messenger.TryAddFriend(friendTwo))
                    {
                        await targetPlayer.Session.SendPacketAsync(new UpdateFriendComposer(friendTwo));
                    }
                }

                await _messengerController.AddFriendAsync(session.Player.Id, targetPlayer.Id);
            }
        }
    }
}
