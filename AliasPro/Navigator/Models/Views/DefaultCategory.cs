using System.Collections.Generic;

namespace AliasPro.Navigator.Models.Views
{
    using Room;
    using Room.Models;

    internal class DefaultCategory : ICategoryType
    {
        public override IList<IRoom> Search(IRoomController roomController, uint categoryId, string searchCode)
        {
            IList<IRoom> roomsToGo = new List<IRoom>();
            ICollection<IRoom> rooms = roomController.GetAllRooms();
            foreach(IRoom room in rooms)
            {
                if (room.RoomData.CategoryId == categoryId)
                    roomsToGo.Add(room);
            }
            return roomsToGo;
        }
    }
}
