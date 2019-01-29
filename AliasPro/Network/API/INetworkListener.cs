using System.Threading.Tasks;

namespace AliasPro.Network
{
    public interface INetworkListener
    {
        Task Listen(int port);
    }
}
