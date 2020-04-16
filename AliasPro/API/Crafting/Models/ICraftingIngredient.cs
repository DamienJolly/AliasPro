namespace AliasPro.API.Crafting.Models
{
	public interface ICraftingIngredient
	{
		int Id { get; set; }
		string Name { get; set; }
		int Amount { get; set; }
	}
}
