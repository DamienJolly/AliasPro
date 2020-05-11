using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;
using System.Threading.Tasks;

namespace AliasPro.Game.Chat.Commands
{
    public class PullCommand : ICommand
    {
        public string[] Names => new[]
        {
            "pull"
        };

        public string PermissionRequired => "cmd_pull";

        public string Parameters => "%target%";

        public string Description => "Pulls the target player towards them.";

        public async Task<bool> TryHandle(ISession session, string[] args)
        {
            if (args.Length <= 0)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter the username of the user you wish to pull.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            string username = args[0];

            if (session.Player.Username == username)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "You cannot pull yourself.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            if (!session.CurrentRoom.Entities.TryGetPlayerEntityByName(username, out BaseEntity targetEntity))
                return false;

            if (!(targetEntity is PlayerEntity playerEntity))
                return false;

            int distanceX = session.Entity.Position.X - playerEntity.Position.X;
            int distanceY = session.Entity.Position.Y - playerEntity.Position.Y;

            if (distanceX < -2 || distanceX > 2 || distanceY < -2 || distanceY > 2)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "That user is too far away to be pulled.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            if (!session.CurrentRoom.RoomGrid.TryGetTileInFront(session.Entity.Position.X, session.Entity.Position.Y, session.Entity.BodyRotation, out IRoomTile targetTile) || 
                !targetTile.IsValidTile(null, true) ||
                targetTile.Position.X == session.CurrentRoom.RoomModel.DoorX && targetTile.Position.Y == session.CurrentRoom.RoomModel.DoorY)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "You cannot pull a user here.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            targetEntity.GoalPosition = new RoomPosition(
                targetTile.Position.X, 
                targetTile.Position.Y, 
                targetTile.Position.Z
            );

            session.CurrentRoom.OnChat("*Pulls " + targetEntity.Name + " towards them*", 0, session.Entity, RoomChatType.SHOUT);

            return true;
        }
    }
}
