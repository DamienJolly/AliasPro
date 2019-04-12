using System.Collections.Generic;

namespace AliasPro.API.Items.Models
{
    public interface IWiredData
    {
        int WiredId { get; set; }
        IList<int> Params { get; set; }
        string Message { get; set; }
        IDictionary<uint, IWiredItemData> Items { get; set; }
        int Delay { get; set; }
    }
}
