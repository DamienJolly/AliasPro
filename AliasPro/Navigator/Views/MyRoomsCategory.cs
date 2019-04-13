using AliasPro.API.Navigator.Views;
using AliasPro.Room;
using AliasPro.Room.Models;
using System.Collections.Generic;

namespace AliasPro.Navigator.Views
{
    internal class MyRoomsCategory : ICategoryType
    {
        //todo: sort this shit out
        private IRoomController _roomController;
        private ICollection<IRoomData> _rooms;

        public override ICollection<IRoomData> Search(IRoomController roomController, uint categoryId, string searchCode, uint playerId)
        {
            _roomController = roomController;
            _rooms = new List<IRoomData>();
            GetPlayerRooms(playerId);

            foreach (IRoomData roomData in _rooms)
            {
                if (roomController.TryGetRoom(roomData.Id, out IRoom room))
                    roomData.UsersNow = room.RoomData.UsersNow;
            }

            return _rooms;
        }

        private async void GetPlayerRooms(uint playerId) =>
            _rooms = await _roomController.GetAllRoomDataById(playerId);
    }
}
