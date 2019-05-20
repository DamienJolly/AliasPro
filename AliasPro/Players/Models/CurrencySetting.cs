using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using System.Data.Common;

namespace AliasPro.Players.Models
{
    internal class CurrencySetting : ICurrencySetting
    {
        public CurrencySetting(DbDataReader reader)
        {
            Id = reader.ReadData<int>("id");
            Time = reader.ReadData<int>("time");
            Amount = reader.ReadData<int>("amount");
        }

        public int Id { get; set; }
        public int Time { get; set; }
        public int Amount { get; set; }
    }
}
