using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.API.Network
{
    public interface INetworkService
    {
        void SetupService(IServiceCollection collection);
    }
}
