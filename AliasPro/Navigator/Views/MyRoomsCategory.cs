using AliasPro.API.Navigator.Views;
using AliasPro.API.Rooms.Models;
using AliasPro.Rooms;
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
                    roomData.UsersNow = room.UsersNow;
            }

            return _rooms;
        }

        private async void GetPlayerRooms(uint playerId) =>
            _rooms = await _roomController.GetPlayersRoomsAsync(playerId);
    }
}
