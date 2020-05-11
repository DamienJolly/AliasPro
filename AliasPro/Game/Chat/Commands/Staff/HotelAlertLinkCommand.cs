using AliasPro.API.Sessions;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;
using AliasPro.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Game.Chat.Commands
{
    internal class HotelAlertLinkCommand : ICommand
    {
        public string[] Names => new[]
        {
            "hotelalertlink",
            "hal"
        };

        public string PermissionRequired => "cmd_hotelalertlink";

        public string Parameters => "%link% %message%";

        public string Description => "Send a message to the entire hotel.";

        private readonly ISessionController _sessionController;

        public HotelAlertLinkCommand(ISessionController sessionController)
        {
            _sessionController = sessionController;
        }

        public async Task<bool> TryHandle(ISession session, string[] args)
        {
            if (args.Length <= 1)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a message to send.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            string link = args[0];

            string message = StringUtils.MergeParams(args, 1, args.Length);

            foreach (ISession targetSession in _sessionController.Sessions)
            {
                //targetSession.SendPacketAsync(new StaffAlertWithLinkComposer(message + "\r\n-" + session.Player.Username, link));

                Dictionary<string, string> keys = new Dictionary<string, string>
                {
                    { "message", message + "\r\n-" + session.Player.Username },
                    { "linkUrl", link },
                    { "linkTitle", link }
                };
                await targetSession.SendPacketAsync(new BubbleAlertComposer("admin", keys));
            }

            return true;
        }
    }
}
