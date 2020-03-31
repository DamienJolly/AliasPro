using AliasPro.API.Chat.Commands;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;
using System.Threading.Tasks;

namespace AliasPro.Chat.Commands
{
    internal class PushCommand : IChatCommand
    {
        public string[] Names => new[]
        {
            "push"
        };

        public string PermissionRequired => "cmd_push";

        public string Parameters => "%target%";

        public string Description => "Pushes the target player away from them.";

        public async Task<bool> Handle(ISession session, string[] args)
        {
            if (args.Length <= 0)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter the username of the user you wish to push.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            string username = args[0];

            if (session.Player.Username == username)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "You cannot push yourself.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            if (!session.CurrentRoom.Entities.TryGetPlayerEntityByName(username, out BaseEntity targetEntity))
                return false;

            if (!(targetEntity is PlayerEntity playerEntity))
                return false;

            if (!session.CurrentRoom.RoomGrid.TryGetTileInFront(session.Entity.Position.X, session.Entity.Position.Y, session.Entity.BodyRotation, out IRoomTile targetTile) || 
                (targetTile.Position.X != playerEntity.Position.X || targetTile.Position.Y != playerEntity.Position.Y))
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "User must be stood infront of you.", 0, 0, RoomChatType.WHISPER));
                return true;
            }


            if (!session.CurrentRoom.RoomGrid.TryGetTileInFront(playerEntity.Position.X, playerEntity.Position.Y, session.Entity.BodyRotation, out IRoomTile newTargetTile) ||
                !newTargetTile.IsValidTile(null, true) ||
                newTargetTile.Position.X == session.CurrentRoom.RoomModel.DoorX && newTargetTile.Position.Y == session.CurrentRoom.RoomModel.DoorY)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "You cannot push a user here.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            targetEntity.GoalPosition = new RoomPosition(
                newTargetTile.Position.X,
                newTargetTile.Position.Y,
                newTargetTile.Position.Z
            );

            session.CurrentRoom.OnChat("*Pushes " + targetEntity.Name + " away from them*", 0, session.Entity, RoomChatType.SHOUT);

            return true;
        }
    }
}
