namespace AliasPro.API.Pets.Models
{
	public interface IPetBreed
	{
		int Race { get; set; }
		int ColourOne { get; set; }
		int ColourTwo { get; set; }
		bool HasColourOne { get; set; }
		bool HasColourTwo { get; set; }
	}
}
