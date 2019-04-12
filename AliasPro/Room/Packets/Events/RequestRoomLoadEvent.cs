using AliasPro.API.Items;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;
using AliasPro.Room.Packets.Composers;
using AliasPro.Sessions;

namespace AliasPro.Room.Packets.Events
{
    public class RequestRoomLoadEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomLoadMessageEvent;

        private readonly IRoomController _roomController;
        private readonly IItemController _itemController;

        public RequestRoomLoadEvent(IRoomController roomController, IItemController itemController)
        {
            _roomController = roomController;
            _itemController = itemController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint roomId = (uint)clientPacket.ReadInt();
            string password = clientPacket.ReadString();

            if (session.CurrentRoom != null)
            {
                await _roomController.RemoveFromRoom(session);
            }

            IRoom room = await _roomController.GetRoomByIdAndPassword(roomId, password);
            if (room != null)
            {
                await session.SendPacketAsync(new RoomOpenComposer());
                await session.SendPacketAsync(new RoomModelComposer(room.RoomModel.Id, room.RoomData.Id));
                await session.SendPacketAsync(new RoomScoreComposer(room.RoomData.Score));
                if (!room.isLoaded)
                {
                    room.isLoaded = true;
                    room.SetupRoomCycle();
                    room.LoadRoomItems(await _itemController.GetItemsForRoomAsync(room.RoomData.Id));
                    room.LoadRoomRights(await _roomController.GetRightsForRoomAsync(room.RoomData.Id));
                }
                
                session.CurrentRoom = room;
            }
            else
            {
                //todo: close connection to room
            }
        }
    }
}
