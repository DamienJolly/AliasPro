using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class IgnoreRoomUserEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.IgnoreRoomUserMessageEvent;

        private readonly IPlayerController _playerController;

        public IgnoreRoomUserEvent(
            IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;


            string username = clientPacket.ReadString();
            if (!room.Entities.TryGetPlayerEntityByName(username, out BaseEntity targetEntity))
                return;

            if (!(targetEntity is PlayerEntity playerEntity)) return;

            if (session.Player.Ignore.TryAdd((int)playerEntity.Player.Id, targetEntity.Name))
            {
                await _playerController.AddPlayerIgnoreAsync((int)session.Player.Id, (int)playerEntity.Player.Id);
                await session.SendPacketAsync(new RoomUserIgnoredComposer(targetEntity.Name, RoomUserIgnoredComposer.IGNORED));
            }
        }
    }
}
