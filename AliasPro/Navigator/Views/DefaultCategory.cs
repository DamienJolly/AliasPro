using AliasPro.API.Navigator.Views;
using AliasPro.Room;
using AliasPro.Room.Models;
using System.Collections.Generic;

namespace AliasPro.Navigator.Views
{
    internal class DefaultCategory : ICategoryType
    {
        public override ICollection<IRoomData> Search(IRoomController roomController, uint categoryId, string searchCode, uint playerId)
        {
            ICollection<IRoomData> roomsToGo = new List<IRoomData>();
            ICollection<IRoom> rooms = roomController.GetAllRooms();

            foreach(IRoom room in rooms)
            {
                if (room.RoomData.CategoryId == categoryId)
                    roomsToGo.Add(room.RoomData);
            }

            return roomsToGo;
        }
    }
}
