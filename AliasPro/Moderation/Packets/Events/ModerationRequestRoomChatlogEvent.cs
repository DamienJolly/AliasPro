using AliasPro.API.Chat;
using AliasPro.API.Moderation;
using AliasPro.API.Moderation.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRequestRoomChatlogEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationRequestRoomChatlogMessageEvent;

        private readonly IModerationController _moderationController;
        private readonly IRoomController _roomController;
        private readonly IChatController _chatController;

        public ModerationRequestRoomChatlogEvent(
            IModerationController moderationController, 
            IRoomController roomController, 
            IChatController chatController)
        {
            _moderationController = moderationController;
            _roomController = roomController;
            _chatController = chatController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: permissions
            if (session.Player.Rank <= 2)
                return;

            clientPacket.ReadInt(); //dunno?
            int roomId = clientPacket.ReadInt();

            IRoomData roomData = await _roomController.ReadRoomDataAsync((uint)roomId);
            if (roomData == null)
                return;

            await session.SendPacketAsync(new ModerationRoomChatlogComposer(roomData, 
                await _chatController.ReadRoomChatlogs(roomData.Id)));
        }
    }
}