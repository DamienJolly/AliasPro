using AliasPro.API.Chat.Commands;
using AliasPro.API.Sessions;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Players.Packets.Composers;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;
using System.Text;
using System.Threading.Tasks;

namespace AliasPro.Chat.Commands
{
    internal class HotelAlertCommand : IChatCommand
    {
        public string[] Names => new[] 
        { 
            "hotelalert", 
            "ha" 
        };

        public string PermissionRequired => "cmd_hotelalert";

        public string Parameters => "%message%";

        public string Description => "Send a message to the entire hotel.";

        private readonly ISessionController _sessionController;

        public HotelAlertCommand(ISessionController sessionController)
        {
            _sessionController = sessionController;
        }

        public async Task<bool> Handle(ISession session, string[] args)
        {
            if (args.Length <= 0)
            {
                await session.SendPacketAsync(new AvatarChatComposer(session.Entity.Id, "Please enter a message to send.", 0, 1, RoomChatType.WHISPER));
                return true;
            }

            StringBuilder message = new StringBuilder();
            //todo: probably some util
            for (int i = 0; i < args.Length; i++) {
                message.Append(args[i]).Append(" ");
            }

            foreach (ISession targetSession in _sessionController.Sessions)
            {
                //targetSession.SendPacketAsync(new StaffAlertWithLinkComposer(message + "\r\n-" + session.Player.Username, ""));
                //targetSession.SendPacketAsync(new BroadcastMessageAlertComposer(message + "\r\n-" + session.Player.Username, ""));
                await targetSession.SendPacketAsync(new BubbleAlertComposer("admin", message + "\r\n-" + session.Player.Username));
            }

            return true;
        }
    }
}
