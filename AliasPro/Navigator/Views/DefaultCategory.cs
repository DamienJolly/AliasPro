using AliasPro.API.Navigator.Views;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using System.Collections.Generic;

namespace AliasPro.Navigator.Views
{
    internal class DefaultCategory : ICategoryType
    {
        public override ICollection<IRoomData> Search(IRoomController roomController, uint categoryId, string searchCode, uint playerId)
        {
            ICollection<IRoomData> roomsToGo = new List<IRoomData>();
            ICollection<IRoom> rooms = roomController.Rooms;

            foreach(IRoom room in rooms)
            {
                if (room.CategoryId == categoryId)
                    roomsToGo.Add(room);
            }

            return roomsToGo;
        }
    }
}
