using AliasPro.API.Badge;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Badges
{
    internal class BadgeService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<BadgeDao>();
            collection.AddSingleton<IBadgeController, BadgeController>();
		}
	}
}
