using AliasPro.API.Currency;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Currency
{
    internal class CurrencyService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<CurrencyDao>();
            collection.AddSingleton<ICurrencyController, CurrencyController>();
        }
    }
}
