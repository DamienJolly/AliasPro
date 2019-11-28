using AliasPro.API.Database;
using AliasPro.API.Figure.Models;
using System.Data.Common;

namespace AliasPro.Figure.Models
{
    internal class ClothingItem : IClothingItem
	{
        public ClothingItem(DbDataReader reader)
        {
			ClothingId = reader.ReadData<int>("clothing_Id");
			Name = reader.ReadData<string>("name");
		}

		public ClothingItem(int clothingId, string name)
        {
			ClothingId = clothingId;
			Name = name;
        }

        public int ClothingId { get; set; }
        public string Name { get; set; }
    }
}