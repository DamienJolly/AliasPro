using AliasPro.API.Database;
using AliasPro.API.Figure.Models;
using System.Collections.Generic;
using System.Data.Common;

namespace AliasPro.Figure.Models
{
    internal class ClothingItem : IClothingItem
	{
        public ClothingItem(DbDataReader reader)
        {
			Id = reader.ReadData<int>("id");
			Name = reader.ReadData<string>("name");
			ClothingIds = new List<int>();

			string setItems = reader.ReadData<string>("setid");
			foreach (string setItem in setItems.Split(','))
			{
				if (int.TryParse(setItem, out int itemId))
					ClothingIds.Add(itemId);
			}
		}

		public ClothingItem(int id, string name, IList<int> clothingIds)
        {
			Id = id;
			Name = name;
			ClothingIds = clothingIds;
		}

        public int Id { get; set; }
		public string Name { get; set; }
		public IList<int> ClothingIds { get; set; }
    }
}