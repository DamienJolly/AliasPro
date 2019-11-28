using System.Collections.Generic;

namespace AliasPro.API.Figure.Models
{
	public interface IClothingItem
	{
		int Id { get; set; }
		string Name { get; set; }
		IList<int> ClothingIds { get; set; }
	}
}
