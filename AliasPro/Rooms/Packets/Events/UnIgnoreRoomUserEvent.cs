using AliasPro.API.Players;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class UnIgnoreRoomUserEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.UnIgnoreRoomUserMessageEvent;

        private readonly IPlayerController _playerController;

        public UnIgnoreRoomUserEvent(
            IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;


            string username = clientPacket.ReadString();
            if (!room.Entities.TryGetPlayerEntityByName(username, out BaseEntity targetEntity))
                return;

            if (!(targetEntity is PlayerEntity playerEntity)) return;

            if (session.Player.Ignore.TryRemove((int)playerEntity.Player.Id))
            {
                await _playerController.RemovePlayerIgnoreAsync((int)session.Player.Id, (int)playerEntity.Player.Id);
                await session.SendPacketAsync(new RoomUserIgnoredComposer(targetEntity.Name, RoomUserIgnoredComposer.UNIGNORED));
            }
        }
    }
}
