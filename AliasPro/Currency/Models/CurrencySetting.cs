using AliasPro.API.Currency.Models;
using AliasPro.API.Database;
using System.Data.Common;

namespace AliasPro.Currency.Models
{
    internal class CurrencySetting : ICurrencySetting
    {
        public CurrencySetting(DbDataReader reader)
        {
            Id = reader.ReadData<int>("id");
            Time = reader.ReadData<int>("time");
            Amount = reader.ReadData<int>("amount");
            Maximum = reader.ReadData<int>("maximum");
            CyclesPerDay = reader.ReadData<int>("cycles_per_day");
        }

        public int Id { get; set; }
        public int Time { get; set; }
        public int Amount { get; set; }
        public int Maximum { get; set; }
        public int CyclesPerDay { get; set; }
    }
}
