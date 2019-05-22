using AliasPro.API.Network;
using AliasPro.API.Permissions;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Permissions
{
    internal class PermissionsService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<PermissionsDao>();
            collection.AddSingleton<IPermissionsController, PermissionsController>();
        }
    }
}
