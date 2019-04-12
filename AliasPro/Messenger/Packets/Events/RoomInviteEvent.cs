using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Player.Models;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Players;
using AliasPro.Sessions;
using System.Collections.Generic;

namespace AliasPro.Messenger.Packets.Events
{
    public class RoomInviteEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomInviteMessageEvent;

        private readonly IPlayerController _playerController;

        public RoomInviteEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: muted
            /*if ()
            {
                await session.SendPacketAsync(new RoomInviteErrorComposer(RoomInviteErrorComposer.YOU_ARE_MUTED, -1));
                return;
            }*/

            IList<IPlayer> targetPlayers = new List<IPlayer>();
            int amount = clientPacket.ReadInt();

            for (int i = 0; i < amount; i++)
            {
                uint targetId = (uint)clientPacket.ReadInt();

                if (_playerController.TryGetPlayer(targetId, out IPlayer targetPlayer))
                    targetPlayers.Add(targetPlayer);
            }

            string message = clientPacket.ReadString();

            if (string.IsNullOrWhiteSpace(message)) return;

            if (message.Length > 200)
                message.Substring(0, 200);

            foreach (IPlayer targetPlayer in targetPlayers)
            {
                if (!session.Player.Messenger.TryGetFriend(targetPlayer.Id, out IMessengerFriend friend))
                {
                    await session.SendPacketAsync(new RoomInviteErrorComposer(RoomInviteErrorComposer.NO_FRIENDS, targetPlayer.Id));
                    return;
                }

                if (targetPlayer.Session != null)
                {
                    //todo: settings ignore invites
                    /*if ()
                    {
                        await session.SendPacketAsync(new RoomInviteErrorComposer(RoomInviteErrorComposer.FRIEND_BUSY, targetPlayer.Id));
                        continue;
                    }*/

                    //todo: muted
                    /*if ()
                    {
                        await session.SendPacketAsync(new RoomInviteErrorComposer(RoomInviteErrorComposer.FRIEND_MUTED, targetPlayer.Id));
                        return;
                    }*/

                    //todo: log invite
                    await targetPlayer.Session.SendPacketAsync(new RoomInviteComposer(session.Player.Id, message));
                }
            }
        }
    }
}
