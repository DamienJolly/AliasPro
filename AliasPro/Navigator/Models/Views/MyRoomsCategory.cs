using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Models.Views
{
    using Room;
    using Room.Models;

    internal class MyRoomsCategory : ICategoryType
    {
        public override async Task<ICollection<IRoomData>> Search(IRoomController roomController, uint categoryId, string searchCode, uint playerId)
        {
            ICollection<IRoomData> roomsToGo =
                await roomController.GetAllRoomDataById(playerId);

            foreach (IRoomData roomData in roomsToGo)
            {
                if (roomController.TryGetRoom(roomData.Id, out IRoom room))
                    roomData.UsersNow = room.RoomData.UsersNow;
            }

            return roomsToGo;
        }
    }
}
