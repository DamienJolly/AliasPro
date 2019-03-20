using System.Collections.Generic;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    internal class WiredData : IWiredData
    {
        internal WiredData(int wiredId, string extraData)
        {
            WiredId = wiredId;
            Params = new List<int>();
            Message = "";
            Items = new List<uint>();
            Delay = 0;

            LoadData(extraData);
        }

        private void LoadData(string extraData)
        {
            string[] parts = extraData.Split(";");

            if (parts.Length != 4) return;

            foreach (string param in parts[0].Split(","))
            {
                if (!int.TryParse(param, out int paramId)) continue;

                Params.Add(paramId);
            }

            Message = parts[1]; //todo: filter

            foreach (string itemString in parts[2].Split(","))
            {
                if (!uint.TryParse(itemString, out uint itemId)) continue;

                Items.Add(itemId);
            }

            if (int.TryParse(parts[3], out int delay))
                Delay = delay;
        }

        public string DataToString =>
            string.Join(",", Params) + ";" + Message + ";" + string.Join(",", Items) + ";" + Delay;

        public int WiredId { get; set; }
        public List<int> Params { get; set; }
        public string Message { get; set; }
        public List<uint> Items { get; set; }
        public int Delay { get; set; }
    }

    public interface IWiredData
    {
        string DataToString { get; }

        int WiredId { get; set; }
        List<int> Params { get; set; }
        string Message { get; set; }
        List<uint> Items { get; set; }
        int Delay { get; set; }
    }
}
