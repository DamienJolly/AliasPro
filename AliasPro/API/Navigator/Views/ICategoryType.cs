using AliasPro.API.Rooms.Models;
using AliasPro.Rooms;
using System.Collections.Generic;

namespace AliasPro.API.Navigator.Views
{
    public abstract class ICategoryType
    {
        public abstract ICollection<IRoomData> Search(IRoomController roomController, uint categoryId, string searchCode, uint playerId);
    }
}
