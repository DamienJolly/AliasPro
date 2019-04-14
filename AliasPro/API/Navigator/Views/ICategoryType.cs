using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using System.Collections.Generic;

namespace AliasPro.API.Navigator.Views
{
    public abstract class ICategoryType
    {
        public abstract ICollection<IRoomData> Search(IRoomController roomController, uint categoryId, string searchCode, uint playerId);
    }
}
