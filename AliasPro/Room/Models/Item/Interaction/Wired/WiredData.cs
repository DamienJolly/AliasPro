using System.Collections.Generic;
using AliasPro.Item.Models;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    internal class WiredData : IWiredData
    {
        internal WiredData(IItem wiredItem)
        {
            Timer = 0;
            Message = "";
            Items = new List<uint>();

            string[] parts = wiredItem.ExtraData.Split(";");

            if (parts.Length != 3) return;
            
            if (int.TryParse(parts[0], out int timer))
                Timer = timer;
            
            Message = parts[1];

            foreach (string itemString in parts[2].Split(","))
            {
                if (!int.TryParse(itemString, out int itemId)) continue;
                
                Items.Add((uint)itemId);
            }
        }
        
        public int Timer { get; set; }
        public string Message { get; set; }
        public List<uint> Items { get; set; }
        
        public string DataToString =>
            Timer + ";" + Message + ";" + string.Join(",", Items);
    }

    public interface IWiredData
    {
        int Timer { get; set; }
        string Message { get; set; }
        List<uint> Items { get; set; }
        string DataToString { get; }
    }
}
