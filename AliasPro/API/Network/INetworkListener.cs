using System.Threading.Tasks;

namespace AliasPro.API.Network
{
    public interface INetworkListener
    {
        Task Listen(int port);
    }
}
