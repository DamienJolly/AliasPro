﻿using AliasPro.API.Chat.Commands;
using AliasPro.API.Moderation;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Players.Models;
using AliasPro.Players.Types;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;
using AliasPro.Utilities;
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
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a username of the target user you wish to give to.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            string username = args[0];
            IPlayer targetPlayer = await _playerController.GetPlayerByUsernameAsync(username);
            if (targetPlayer == null)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Player cannot be found or is not online.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            if (targetPlayer.Rank >= session.Player.Rank)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "You cannot ban this user sorry!.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            if (args.Length <= 1)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a type of ban you wish to issue.", 0, 1, RoomChatType.WHISPER));
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
                        //todo: ip banning
                        duration = 78892200;

                        if (args.Length <= 2)
                        {
                            await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a reason for your ban.", 0, 1, RoomChatType.WHISPER));
                            return true;
                        }

                        reason = StringUtils.MergeParams(args, 2, args.Length);
                        break;
                    }
                case "mac":
                case "machine":
                    {
                        //todo: machine banning
                        duration = 78892200;

                        if (args.Length <= 2)
                        {
                            await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a reason for your ban.", 0, 1, RoomChatType.WHISPER));
                            return true;
                        }

                        reason = StringUtils.MergeParams(args, 2, args.Length);
                        break;
                    }
                default:
                    {
                        if (!int.TryParse(query, out duration))
                        {
                            await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a valid duration for the ban.", 0, 1, RoomChatType.WHISPER));
                            return true;
                        }

                        if (args.Length <= 2)
                        {
                            await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a reason for your ban.", 0, 1, RoomChatType.WHISPER));
                            return true;
                        }

                        reason = StringUtils.MergeParams(args, 2, args.Length);
                        break;
                    }
            }

            if (duration <= 0)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a valid duration for the ban.", 0, 1, RoomChatType.WHISPER));
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
