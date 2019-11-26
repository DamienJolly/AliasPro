namespace AliasPro.API.Badges.Models
{
	public interface IBadge
	{
		int Id { get; set; }
		string Code { get; set; }
		string RequiredRight { get; set; }
	}
}
