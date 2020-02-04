using AliasPro.API.Permissions;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Permissions
{
    internal class PermissionsService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<PermissionsDao>();
            collection.AddSingleton<IPermissionsController, PermissionsController>();
        }
    }
}
