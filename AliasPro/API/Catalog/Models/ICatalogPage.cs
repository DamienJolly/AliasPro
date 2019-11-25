using AliasPro.API.Catalog.Layouts;
using System.Collections.Generic;

namespace AliasPro.API.Catalog.Models
{
    public interface ICatalogPage
    {
        int Id { get; }
        int ParentId { get; }
        string Name { get; }
        string Caption { get; }
        int Icon { get; }
        int Rank { get; }
        int Order { get; }
        IDictionary<int, ICatalogItem> Items { get; set; }
		string HeaderImage { get; }
        string TeaserImage { get; }
        string SpecialImage { get; }
        string TextOne { get; }
        string TextTwo { get; }
        string TextDetails { get; }
        string TextTeaser { get; }
        ICatalogLayout Layout { get; }
        bool Enabled { get; }
        bool Visible { get; }

        bool TryGetCatalogItem(int itemId, out ICatalogItem item);

	}
}
