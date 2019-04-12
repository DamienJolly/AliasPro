using AliasPro.API.Items.Models;
using System.Collections.Generic;

namespace AliasPro.Items.Models
{
    internal class WiredData : IWiredData
    {
        internal WiredData(int wiredId, string extraData)
        {
            WiredId = wiredId;
            Params = new List<int>();
            Message = string.Empty;
            Items = new Dictionary<uint, IWiredItemData>();
            Delay = 0;

            LoadData(extraData);
        }

        private void LoadData(string extraData)
        {
            string[] parts = extraData.Split(";");
            if (parts.Length != 4) return;

            foreach (string param in parts[0].Split(","))
            {
                if (string.IsNullOrEmpty(param)) continue;

                Params.Add(int.Parse(param));
            }

            Message = parts[1]; //todo: filter

            foreach (string itemString in parts[2].Split(","))
            {
                string[] itemParts = itemString.Split(":");
                if (itemParts.Length != 7) continue;

                Items.Add(uint.Parse(itemParts[0]), new WiredItemData(itemParts));
            }

            Delay = int.Parse(parts[3]);
        }

        public override string ToString()
        {
            return string.Join(",", Params) + ";" + Message + ";" + string.Join(",", Items.Values) + ";" + Delay;
        }

        public int WiredId { get; set; }
        public IList<int> Params { get; set; }
        public string Message { get; set; }
        public IDictionary<uint, IWiredItemData> Items { get; set; }
        public int Delay { get; set; }
    }
}
