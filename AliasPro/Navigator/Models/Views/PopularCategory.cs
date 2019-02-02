using System.Collections.Generic;

namespace AliasPro.Navigator.Models.Views
{
    using Room;
    using Room.Models;

    internal class PopularCategory : ICategoryType
    {
        public override IList<IRoom> Search(IRoomController roomController, uint categoryId, string searchCode)
        {
            IList<IRoom> roomsToGo = new List<IRoom>();
            ICollection<IRoom> rooms = roomController.GetAllRooms();
            foreach (IRoom room in rooms)
            {
                roomsToGo.Add(room);
            }
            return roomsToGo;
        }
    }
}
