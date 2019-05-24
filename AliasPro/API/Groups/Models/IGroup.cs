namespace AliasPro.API.Groups.Models
{
	public interface IGroup
	{
		int Id { get; set; }
		string Name { get; set; }
		string Description { get; set; }
		int OwnerId { get; set; }
		int CreatedAt { get; set; }
		int RoomId { get; set; }
		string Badge { get; set; }
		int ColourOne { get; set; }
		int ColourTwo { get; set; }
	}
}
