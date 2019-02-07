using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Models;
    using Packets.Outgoing;
    using Models.Messenger;
    using System.Collections.Generic;

    public class RoomInviteEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomInviteMessageEvent;

        private readonly IPlayerController _playerController;

        public RoomInviteEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
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
                IPlayer targetPlayer = 
                    await _playerController.GetPlayerByIdAsync(targetId);

                if (targetPlayer != null)
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
