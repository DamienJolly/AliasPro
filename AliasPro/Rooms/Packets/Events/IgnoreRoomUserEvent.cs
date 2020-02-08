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
    public class IgnoreRoomUserEvent : IMessageEvent
    {
        public short Header => Incoming.IgnoreRoomUserMessageEvent;

        private readonly IPlayerController _playerController;

        public IgnoreRoomUserEvent(
            IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;


            string username = message.ReadString();
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
