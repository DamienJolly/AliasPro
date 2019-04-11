using AliasPro.API.Player.Models;
using AliasPro.Database;
using System.Data.Common;

namespace AliasPro.Player.Models
{
    internal class PlayerCurrency : IPlayerCurrency
    {
        public PlayerCurrency(DbDataReader reader)
        {
            Type = reader.ReadData<int>("type");
            Amount = reader.ReadData<int>("amount");
        }

        public int Type { get; }
        public int Amount { get; }
    }
}
