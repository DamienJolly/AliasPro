using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserBanEvent : IMessageEvent
    {
        public short Header => Incoming.RoomUserBanMessageEvent;

        private readonly IRoomController _roomController;
        private readonly IPlayerController _playerController;

        public RoomUserBanEvent(
            IRoomController roomController,
            IPlayerController playerController)
        {
            _roomController = roomController;
            _playerController = playerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int playerId = message.ReadInt();
            int roomId = message.ReadInt();
            string banName = message.ReadString();

            if (!_roomController.TryGetRoom((uint)roomId, out IRoom room)) 
                return;

            switch (room.Settings.WhoBans)
            {
                case 0: default: if (!room.Rights.IsOwner(session.Player.Id)) return; break;
                case 1: if (!room.Rights.HasRights(session.Player.Id)) return; break;
            }

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
