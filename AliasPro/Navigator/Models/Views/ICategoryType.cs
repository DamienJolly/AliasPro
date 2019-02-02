using System.Collections.Generic;

namespace AliasPro.Navigator.Models.Views
{
    using Room;
    using Room.Models;

    public abstract class ICategoryType
    {
        public abstract IList<IRoom> Search(IRoomController roomController, uint categoryId, string searchCode);
    }
}
