using System.Collections.Generic;

namespace AliasPro.Navigator.Models.Views
{
    using Room;
    using Room.Models;

    internal class TopPromotionsCategory : ICategoryType
    {
        public override IList<IRoom> Search(IRoomController roomController, uint categoryId, string searchCode)
        {
            IList<IRoom> roomsToGo = new List<IRoom>();
            return roomsToGo;
        }
    }
}
