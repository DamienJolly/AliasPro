using AliasPro.API.Chat.Commands;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Players.Packets.Composers;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Chat.Commands
{
    internal class GiveCommand : IChatCommand
    {
        public string[] Names => new[]
        {
            "give"
        };

        public string PermissionRequired => "cmd_give";

        public string Parameters => "%target% %query% (%type%) %value%";

        public string Description => "Give the target user credts, badges, points ext";

        private IPlayerController _playerController;

        public GiveCommand(
            IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task<bool> Handle(ISession session, string[] args)
        {
            if (args.Length <= 0)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a username of the target user you wish to give to.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            string username = args[0];

            if (!_playerController.TryGetPlayer(username, out IPlayer targetPlayer))
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Player cannot be found or is not online.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            if (args.Length <= 1)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a query or type \":give list\" for a list of availabe quries.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            string query = args[1];

            if (query == "list")
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Harrass Damien to add a list here.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            int points = -1;

            if (query == "points")
            {
                if (args.Length <= 2)
                {
                    await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter the points type you wish to give to the target user.", 0, 0, RoomChatType.WHISPER));
                    return true;
                }

                if (!int.TryParse(args[2], out points))
                {
                    await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "You have no entered a valid points type. Type must be an integer.", 0, 0, RoomChatType.WHISPER));
                    return true;
                }

                //todo: some check to see if currency exists

                args = args.Skip(1).ToArray();
            }

            if (args.Length <= 2)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter value to give to the target user.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            string value = args[2];

            switch (query)
            {
                case "credits":
                case "points":
                    {
                        if (!int.TryParse(value, out int amount))
                        {
                            await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "You have no entered a valid credits amount. Amount must be an integer.", 0, 0, RoomChatType.WHISPER));
                            return true;
                        }

                        IPlayerCurrency currency = 
                            await targetPlayer.GetPlayerCurrency(points);

                        if (currency != null)
                        {
                            currency.Amount += amount;
                            if (points == -1)
                            {
                                await targetPlayer.Session.SendPacketAsync(new UserCreditsComposer(currency.Amount));
                            }
                            else
                            {
                                await targetPlayer.Session.SendPacketAsync(new UserPointsComposer(currency.Amount, amount, currency.Type));
                            }
                        }

                        break;
                    }
                case "badge":
                    {
                        await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Tell Damien to code this, thanks!.", 0, 0, RoomChatType.WHISPER));
                        return true;
                    }
            }

            return true;
        }
    }
}
