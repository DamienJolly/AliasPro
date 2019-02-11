using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Models.Views
{
    using Room;
    using Room.Models;

    internal class MyRoomsCategory : ICategoryType
    {
        public override async Task<ICollection<IRoomData>> Search(IRoomController roomController, uint categoryId, string searchCode, uint playerId) =>
            await roomController.GetAllRoomDataById(playerId);
    }
}
