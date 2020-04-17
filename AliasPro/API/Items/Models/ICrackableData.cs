using System.Collections.Generic;

namespace AliasPro.API.Items.Models
{
    public interface ICrackableData
    {
        int ItemId { get; }
        int Count { get; }
        IDictionary<int, int> Prizes { get; }
        int TotalChance { get; }
        int CalculateCrackState(int hits, int states);
        bool TryGetCrackableReward(out int prizeId);
    }
}
