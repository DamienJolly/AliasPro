using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Network
{
    public interface INetworkService
    {
        void SetupService(IServiceCollection collection);
    }
}
