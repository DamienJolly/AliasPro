using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Room
{
    using Sessions;
    using Models;
    using Models.Entities;

    public interface IRoomController
    {
        BaseEntity AddUserToRoom(IRoom room, ISession session);
        Task<IRoom> GetRoomByIdAsync(int id);
        Task<IRoom> GetRoomByIdAndPassword(int id, string password);
        IList<IRoom> GetRoomsByCategorySearch(uint categoryId, string searchCode);
    }
}
