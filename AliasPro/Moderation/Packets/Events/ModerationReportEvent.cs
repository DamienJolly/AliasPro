using AliasPro.API.Moderation;
using AliasPro.API.Moderation.Models;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Moderation.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Moderation.Types;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationReportEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationReportMessageEvent;
        
		private readonly IModerationController _moderationController;
		private readonly IPlayerController _playerController;
		private readonly IRoomController _roomController;

		public ModerationReportEvent(
            IModerationController moderationController,
            IPlayerController playerController,
            IRoomController roomController)
        {
            _moderationController = moderationController;
            _playerController = playerController;
            _roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            string msg = message.ReadString();
            int topicId = message.ReadInt();
            int playerId = message.ReadInt();
            int roomId = message.ReadInt();
            int messageCount = message.ReadInt(); //todo: code hightlighted messages

            if (!_moderationController.TryGetTopic(topicId, out IModerationTopic topic))
                return;

            IModerationTicket ticket;
            if (playerId != -1)
            {
                IPlayer targetPlayer = await _playerController.GetPlayerAsync((uint)playerId);
                if (targetPlayer == null)
                    return;

                ticket = new ModerationTicket(
                    (int)session.Player.Id,
                    session.Player.Username,
                    (int)targetPlayer.Id,
                    targetPlayer.Username,
                    roomId,
                    msg,
                    topicId,
                    ModerationTicketType.NORMAL);
            }
            else
            {
                if (!_roomController.TryGetRoom((uint)roomId, out IRoom room))
                    return;

                ticket = new ModerationTicket(
                    (int)session.Player.Id,
                    session.Player.Username,
                    (int)room.OwnerId,
                    room.OwnerName,
                    roomId,
                    msg,
                    topicId,
                    ModerationTicketType.ROOM);
            }

            ticket.Id = await _moderationController.AddTicket(ticket);
            _moderationController.TryAddTicket(ticket);
            await session.SendPacketAsync(new ModerationReportReceivedAlertComposer(ModerationReportReceivedAlertComposer.REPORT_RECEIVED, topic.Reply));
        }
    }
}