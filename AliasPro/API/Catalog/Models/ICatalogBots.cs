using AliasPro.Players.Types;

namespace AliasPro.API.Catalog.Models
{
    public interface ICatalogBot
    {
        int ItemId { get; }
        string Name { get; }
		string Motto { get; }
		string Figure { get; }
		PlayerGender Gender { get; }
    }
}
