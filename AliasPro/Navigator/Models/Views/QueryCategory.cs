using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Models.Views
{
    using Room;
    using Room.Models;

    internal class QueryCategory : ICategoryType
    {
        public override Task<ICollection<IRoom>> Search(IRoomController roomController, uint categoryId, string searchCode, uint playerId)
        {
            ICollection<IRoom> roomsToGo = new List<IRoom>();
            ICollection<IRoom> rooms = roomController.GetAllRooms();
            foreach (IRoom room in rooms)
            {
                roomsToGo.Add(room);
            }
            return Task.FromResult(roomsToGo);
        }
    }
}
