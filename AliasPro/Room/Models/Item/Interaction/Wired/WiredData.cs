using System.Collections.Generic;
using AliasPro.Item.Models;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    internal class WiredData : IWiredData
    {
        internal WiredData(string extraData)
        {
            Timer = 0;
            Message = "";
            Items = new List<IItem>();

            string[] parts = extraData.Split(";");

            if (parts.Length != 3) return;
            
            if (int.TryParse(parts[0], out int timer))
                Timer = timer;
            
            Message = parts[1];
        }
        
        public int Timer { get; set; }
        public string Message { get; set; }
        public List<IItem> Items { get; set; }

        public string DataToString =>
            Timer + ";" + Message + ";";
    }

    public interface IWiredData
    {
        int Timer { get; set; }
        string Message { get; set; }
        List<IItem> Items { get; set; }
        string DataToString { get; }
    }
}
