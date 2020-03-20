using AliasPro.API.Chat.Commands;
using AliasPro.API.Sessions;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Players.Packets.Composers;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;
using System.Collections.Generic;
using System.Text;

namespace AliasPro.Chat.Commands
{
    internal class HotelAlertLinkCommand : IChatCommand
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

        public bool Handle(ISession session, string[] args)
        {
            if (args.Length <= 1)
            {
                session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a message to send.", 0, 0, RoomChatType.WHISPER));
                return true;
            }

            string link = args[0];

            StringBuilder message = new StringBuilder();
            //todo: probably some util
            for (int i = 1; i < args.Length; i++) {
                message.Append(args[i]).Append(" ");
            }

            foreach (ISession targetSession in _sessionController.Sessions)
            {
                //targetSession.SendPacketAsync(new StaffAlertWithLinkComposer(message + "\r\n-" + session.Player.Username, link));

                Dictionary<string, string> keys = new Dictionary<string, string>
                {
                    { "message", message + "\r\n-" + session.Player.Username },
                    { "linkUrl", link },
                    { "linkTitle", link }
                };
                targetSession.SendPacketAsync(new BubbleAlertComposer("admin", keys));
            }

            return true;
        }
    }
}
