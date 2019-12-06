using System.Collections.Generic;

namespace AliasPro.API.Pets.Models
{
	public interface IPetData
	{
		int Type { get; set; }
		string Name { get; set; }
		IList<IPetBreed> Breeds { get; set; }
	}
}
