using System.Collections.Generic;
using AliasPro.Item.Models;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    internal class WiredData : IWiredData
    {
        internal WiredData(IItem wiredItem)
        {
            Timer = 0;
            OwnerOnly = false;
            Message = "";
            Items = new List<uint>();

            string[] parts = wiredItem.ExtraData.Split(";");

            if (parts.Length != 4) return;
            
            if (int.TryParse(parts[0], out int timer))
                Timer = timer;

            if (int.TryParse(parts[1], out int owner))
                OwnerOnly = owner == 1;

            Message = parts[2];

            foreach (string itemString in parts[3].Split(","))
            {
                if (!int.TryParse(itemString, out int itemId)) continue;
                
                Items.Add((uint)itemId);
            }
        }
        
        public int Timer { get; set; }
        public string Message { get; set; }
        public bool OwnerOnly { get; set; }
        public List<uint> Items { get; set; }
        
        public string DataToString =>
            Timer + ";" + Message + ";" + (OwnerOnly ? "1" : "0") + ";" + string.Join(",", Items);
    }

    public interface IWiredData
    {
        int Timer { get; set; }
        string Message { get; set; }
        bool OwnerOnly { get; set; }
        List<uint> Items { get; set; }
        string DataToString { get; }
    }
}
