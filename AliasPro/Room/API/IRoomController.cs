using System.Threading.Tasks;

namespace AliasPro.Room
{
    using Sessions;
    using Models;

    public interface IRoomController
    {
        Task<IRoom> GetRoomByIdAsync(int id);
        Task<IRoom> GetRoomByIdAndPassword(int id, string password);
    }
}
