using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Models.Views
{
    using Room;
    using Room.Models;

    public abstract class ICategoryType
    {
        public abstract Task<ICollection<IRoomData>> Search(IRoomController roomController, uint categoryId, string searchCode, uint playerId);
    }
}
