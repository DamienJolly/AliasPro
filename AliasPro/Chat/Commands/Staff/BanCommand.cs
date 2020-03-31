using AliasPro.API.Chat.Commands;
using AliasPro.API.Moderation;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Players.Models;
using AliasPro.Players.Types;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;
using AliasPro.Utilities;
using System.Text;
using System.Threading.Tasks;

namespace AliasPro.Chat.Commands
{
    internal class BanCommand : IChatCommand
    {
        public string[] Names => new[]
        {
            "ban"
        };

        public string PermissionRequired => "cmd_ban";

        public string Parameters => "%target% %type%/%time% %reason%";

        public string Description => "Bans the target user";

        private IPlayerController _playerController;

        private IModerationController _moderationController;

        public BanCommand(
            IPlayerController playerController,
            IModerationController moderationController)
        {
            _playerController = playerController;
            _moderationController = moderationController;
        }

        public async Task<bool> Handle(ISession session, string[] args)
        {
            if (args.Length <= 0)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a username of the target user you wish to give to.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            string username = args[0];
            IPlayer targetPlayer = await _playerController.GetPlayerByUsernameAsync(username);
            if (targetPlayer == null)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Player cannot be found or is not online.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            if (targetPlayer.Rank >= session.Player.Rank)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "You cannot ban this user sorry!.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            if (args.Length <= 1)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a type of ban you wish to issue.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            string query = args[1];
            int duration;
            string reason;

            switch (query)
            {
                case "ad":
                case "adv":
                case "advert":
                case "advertise":
                case "advertising":
                    {
                        duration = 78892200;
                        reason = "Advertising another hotel.";
                        break;
                    }
                case "ip":
                case "ipaddress":
                    {
                        duration = 78892200;

                        if (args.Length <= 2)
                        {
                            await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a reason for your ban.", 0, 0, RoomChatType.WHISPER));
                            return true;
                        }

                        StringBuilder message = new StringBuilder();
                        //todo: probably some util
                        for (int i = 2; i < args.Length; i++)
                        {
                            message.Append(args[i]).Append(" ");
                        }

                        reason = message.ToString();
                        break;
                    }
                case "mac":
                case "machine":
                    {
                        duration = 78892200;

                        if (args.Length <= 2)
                        {
                            await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a reason for your ban.", 0, 0, RoomChatType.WHISPER));
                            return true;
                        }

                        StringBuilder message = new StringBuilder();
                        //todo: probably some util
                        for (int i = 2; i < args.Length; i++)
                        {
                            message.Append(args[i]).Append(" ");
                        }

                        reason = message.ToString();
                        break;
                    }
                default:
                    {
                        if (!int.TryParse(query, out duration))
                        {
                            await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a valid duration for the ban.", 0, 0, RoomChatType.WHISPER));
                            return true;
                        }

                        if (args.Length <= 2)
                        {
                            await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a reason for your ban.", 0, 0, RoomChatType.WHISPER));
                            return true;
                        }

                        StringBuilder message = new StringBuilder();
                        //todo: probably some util
                        for (int i = 2; i < args.Length; i++)
                        {
                            message.Append(args[i]).Append(" ");
                        }

                        reason = message.ToString();
                        break;
                    }
            }

            if (duration <= 0)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a valid duration for the ban.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            IPlayerSanction sanction = new PlayerSanction(
                SanctionType.BAN,
                duration + (int)UnixTimestamp.Now,
                reason);

            await _playerController.AddPlayerSanction(targetPlayer.Id, sanction);

            if (targetPlayer.Session != null)
            {
                targetPlayer.Sanction.AddSanction(sanction);
                targetPlayer.Session.Disconnect();
            }
            return true;
        }
    }
}
