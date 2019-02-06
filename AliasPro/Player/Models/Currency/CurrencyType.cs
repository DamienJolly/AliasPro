using System.Data.Common;

namespace AliasPro.Player.Models.Currency
{
    using Database;

    internal class CurrencyType : ICurrencyType
    {
        public CurrencyType(DbDataReader reader)
        {
            Type = reader.ReadData<int>("type");
            Amount = reader.ReadData<int>("amount");
        }

        public int Type { get; }
        public int Amount { get; }
    }

    public interface ICurrencyType
    {
        int Type { get; }
        int Amount { get; }
    }
}
