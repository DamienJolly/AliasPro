using AliasPro.API.Messenger.Models;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Players.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Events
{
    public class RoomInviteEvent : IMessageEvent
    {
        public short Header => Incoming.RoomInviteMessageEvent;

        private readonly IPlayerController _playerController;

        public RoomInviteEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            if (session.Player.Sanction.GetCurrentSanction(out IPlayerSanction playerSanction) && playerSanction.Type == SanctionType.MUTE)
            {
                await session.SendPacketAsync(new RoomInviteErrorComposer(RoomInviteErrorComposer.YOU_ARE_MUTED, session.Player.Id));
                return;
            }

            IList<IPlayer> targetPlayers = new List<IPlayer>();
            int amount = message.ReadInt();

            for (int i = 0; i < amount; i++)
            {
                uint targetId = (uint)message.ReadInt();

                if (_playerController.TryGetPlayer(targetId, out IPlayer targetPlayer))
                    targetPlayers.Add(targetPlayer);
            }

            string msg = message.ReadString();

            if (string.IsNullOrWhiteSpace(msg)) return;

            if (msg.Length > 200)
                msg.Substring(0, 200);

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

                    if (targetPlayer.Sanction.GetCurrentSanction(out IPlayerSanction targetSanction) && targetSanction.Type == SanctionType.MUTE)
                    {
                        await session.SendPacketAsync(new RoomInviteErrorComposer(RoomInviteErrorComposer.FRIEND_MUTED, targetPlayer.Id));
                        return;
                    }

                    //todo: log invite
                    await targetPlayer.Session.SendPacketAsync(new RoomInviteComposer(session.Player.Id, msg));
                }
            }
        }
    }
}
