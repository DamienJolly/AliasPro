using AliasPro.API.Navigator;
using AliasPro.API.Rooms;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestCreateRoomEvent : IMessageEvent
    {
        public short Header => Incoming.RequestCreateRoomMessageEvent;

        private readonly IRoomController _roomController;
        private readonly INavigatorController _navigatorController;

        public RequestCreateRoomEvent(
            IRoomController roomController, 
            INavigatorController navigatorController)
        {
            _roomController = roomController;
            _navigatorController = navigatorController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            string name = message.ReadString();
            string description = message.ReadString();
            string modelName = message.ReadString();
            int categoryId = message.ReadInt();
            int maxUsers = message.ReadInt();
            int tradeType = message.ReadInt();

            if (name.Length > 60)
                name.Substring(0, 60);

            if (name.Length <= 0) return;

            if (!_roomController.TryGetRoomModel(modelName, out _)) return;

            // todo: fix
            //if (!_navigatorController.TryGetRoomCategory((uint)categoryId, out INavigatorCategory category)) return;

            if (maxUsers > 250 || maxUsers < 10) return;

            if (tradeType > 2 || tradeType < 0) return;

            // todo: room count check

            int roomId = await _roomController.CreateRoomAsync(session.Player.Id, name, description, modelName, categoryId, maxUsers, tradeType);
            await session.SendPacketAsync(new RoomCreatedComposer(roomId, name));
        }
    }
}
