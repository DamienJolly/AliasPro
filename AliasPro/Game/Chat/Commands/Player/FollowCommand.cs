using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;
using System.Threading.Tasks;

namespace AliasPro.Game.Chat.Commands
{
    public class FollowCommand : ICommand
    {
        public string[] Names => new[]
        {
            "follow",
            "stalk",
            "goto"
        };

        public string PermissionRequired => "cmd_follow";

        public string Parameters => "";

        public string Description => "Hello world!";

        private readonly IPlayerController _playerController;

        public FollowCommand(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task<bool> TryHandle(ISession session, string[] args)
        {
            if (args.Length <= 0)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter the username of the user you wish to follow.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            string username = args[0];

            if (session.Player.Username == username)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "You cannot follow yourself.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            if (!_playerController.TryGetPlayer(username, out IPlayer targetPlayer))
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Player cannot be found or is not online.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            if (targetPlayer.Session?.CurrentRoom == null)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Player cannot be followed.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            if (targetPlayer.Session.CurrentRoom == session.CurrentRoom)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Player is already in this room.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            await session.SendPacketAsync(new ForwardToRoomComposer(targetPlayer.Session.CurrentRoom.Id));
            return true;
        }
    }
}
