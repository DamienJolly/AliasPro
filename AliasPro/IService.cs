using Microsoft.Extensions.DependencyInjection;

namespace AliasPro
{
    public interface IService
    {
        void Register(IServiceCollection serviceDescriptors);
    }
}
