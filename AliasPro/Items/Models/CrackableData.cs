using AliasPro.API.Database;
using AliasPro.API.Items.Models;
using AliasPro.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace AliasPro.Items.Models
{
    internal class CrackableData : ICrackableData
    {
        internal CrackableData(DbDataReader reader)
        {
            ItemId = reader.ReadData<int>("item_id");
            Count = reader.ReadData<int>("count");
            EffectId = reader.ReadData<int>("effect_id");
            Prizes = new Dictionary<int, int>();

            TotalChance = 0;
            foreach (string prize in reader.ReadData<string>("prizes").Split(";"))
            {
                int itemId = 0;
                int chance = 100;

                if (prize.Contains(":") && prize.Split(":").Length == 2)
                {
                    itemId = int.Parse(prize.Split(":")[0]);
                    chance = int.Parse(prize.Split(":")[1]);
                }
                else
                {
                    itemId = int.Parse(prize.Replace(":", ""));
                }

                Prizes.Add(itemId, chance);
                TotalChance += chance;
            }
        }

        public int CalculateCrackState(int hits, int states) => 
            (int)Math.Floor(1.0D / ((double)Count / hits) * states);

        public bool TryGetCrackableReward(out int prizeId)
        {
            prizeId = 0;
            if (Prizes.Count == 0)
                return false;

            int random = Randomness.RandomNumber(TotalChance + 1);

            int count = 0;
            foreach (var prize in Prizes)
            {
                if (prize.Value >= random - count)
                {
                    prizeId = prize.Key;
                    return true;
                }

                count += prize.Value;
            }

            return false;
        }

        public int ItemId { get; }
        public int Count { get; }
        public int EffectId { get; }
        public IDictionary<int, int> Prizes { get; }
        public int TotalChance { get; }
    }
}
