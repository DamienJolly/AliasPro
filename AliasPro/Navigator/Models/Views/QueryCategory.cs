using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Models.Views
{
    using Room;
    using Room.Models;

    internal class QueryCategory : ICategoryType
    {
        public override Task<ICollection<IRoomData>> Search(IRoomController roomController, uint categoryId, string searchCode, uint playerId)
        {
            ICollection<IRoomData> roomsToGo = new List<IRoomData>();
            ICollection<IRoom> rooms = roomController.GetAllRooms();
            foreach (IRoom room in rooms)
            {
                roomsToGo.Add(room.RoomData);
            }
            return Task.FromResult(roomsToGo);
        }
    }
}
