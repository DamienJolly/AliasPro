using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Models.Views
{
    using Room;
    using Room.Models;

    internal class PromotionsCategory : ICategoryType
    {
        public override Task<ICollection<IRoom>> Search(IRoomController roomController, uint categoryId, string searchCode, uint playerId)
        {
            ICollection<IRoom> roomsToGo = new List<IRoom>();
            return Task.FromResult(roomsToGo);
        }
    }
}
