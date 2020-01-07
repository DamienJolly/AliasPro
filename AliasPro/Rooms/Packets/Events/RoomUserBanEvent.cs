using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserBanEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomUserBanMessageEvent;

        private readonly IRoomController _roomController;
        private readonly IPlayerController _playerController;

        public RoomUserBanEvent(
            IRoomController roomController,
            IPlayerController playerController)
        {
            _roomController = roomController;
            _playerController = playerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int playerId = clientPacket.ReadInt();
            int roomId = clientPacket.ReadInt();
            string banName = clientPacket.ReadString();

            if (!_roomController.TryGetRoom((uint)roomId, out IRoom room)) 
                return;

            if (!room.Rights.HasRights(session.Player.Id)) return;

            if (room.Rights.HasRights((uint)playerId)) return;

            IPlayerData playerData = await _playerController.GetPlayerDataAsync((uint)playerId);
            if (playerData == null)
                return;

            int time = 0;
            if (banName.ToLower().Contains("hour"))
                time = 3600;
            else if (banName.ToLower().Contains("day"))
                time = 86400;
            else if (banName.ToLower().Contains("perm"))
                time = 78892200;

            IRoomBan roomBan = new RoomBan(
                playerId,
                playerData.Username, 
                (int)UnixTimestamp.Now + time
            );

            if (!room.Bans.TryBanPlayer(roomBan.PlayerId, roomBan))
                return;

            await _roomController.AddRoomBan(room.Id, roomBan.PlayerId, roomBan.ExpireTime);

            if (room.Entities.TryGetPlayerEntityById(playerId, out PlayerEntity entity))
            {
                if (room.RoomModel.IsCustom /*|| room.IsPublic  */)
                {
                    await room.RemoveEntity(entity);
                }
                else
                {
                    entity.Session.CurrentRoom = null;
                    entity.IsKicked = true;

                    entity.GoalPosition = new RoomPosition(
                        room.RoomModel.DoorX,
                        room.RoomModel.DoorY,
                        room.RoomModel.DoorZ
                    );
                }
            }
        }
    }
}
