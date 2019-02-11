using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Models.Views
{
    using Room;
    using Room.Models;

    internal class PromotionsCategory : ICategoryType
    {
        public override Task<ICollection<IRoomData>> Search(IRoomController roomController, uint categoryId, string searchCode, uint playerId)
        {
            ICollection<IRoomData> roomsToGo = new List<IRoomData>();
            return Task.FromResult(roomsToGo);
        }
    }
}
