using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using System.Data.Common;

namespace AliasPro.Players.Models
{
    internal class PlayerCurrency : IPlayerCurrency
    {
        public PlayerCurrency(DbDataReader reader)
        {
            Type = reader.ReadData<int>("type");
            Amount = reader.ReadData<int>("amount");
            Cycles = reader.ReadData<int>("cycles");
        }

        public PlayerCurrency(int type, int amount = 0, int cycles = 0)
        {
            Type = type;
            Amount = amount;
            Cycles = cycles;
        }

        public int Type { get; }
        public int Amount { get; set; }
        public int Cycles { get; set; }
    }
}
