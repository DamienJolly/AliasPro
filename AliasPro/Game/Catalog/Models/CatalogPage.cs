using AliasPro.Game.Catalog.Layouts;
using AliasPro.Game.Catalog.Utilities;
using System.Collections.Generic;

namespace AliasPro.Game.Catalog.Models
{
	public class CatalogPage
	{
        public CatalogPage(int id, int parentId, string name, string caption, int icon, int rank, 
            string headerImage, string teaserImage, string specialImage, string textOne, string textTwo, 
            string textDetails, string textTeaser, string layout, bool enabled, bool visible)
        {
            Id = id;
            ParentId = parentId;
            Name = name;
            Caption = caption;
            Icon = icon;
            Rank = rank;
            HeaderImage = headerImage;
            TeaserImage = teaserImage;
            SpecialImage = specialImage;
            TextOne = textOne;
            TextTwo = textTwo;
            TextDetails = textDetails;
            TextTeaser = textTeaser;
            Layout = CatalogLayoutUtility.GetCatalogLayout(layout, this);
            Enabled = enabled;
            Visible = visible;
            Items = new Dictionary<int, CatalogItem>();
            OfferIds = new List<int>();
        }

        public bool TryGetCatalogItem(int itemId, out CatalogItem item) =>
            Items.TryGetValue(itemId, out item);

        public int Id { get; }
        public int ParentId { get; }
        public string Name { get; }
        public string Caption { get; }
        public int Icon { get; }
        public int Rank { get; }
        public string HeaderImage { get; }
        public string TeaserImage { get; }
        public string SpecialImage { get; }
        public string TextOne { get; }
        public string TextTwo { get; }
        public string TextDetails { get; }
        public string TextTeaser { get; }
        public CatalogLayout Layout { get; }
        public bool Enabled { get; }
        public bool Visible { get; }
        public Dictionary<int, CatalogItem> Items { get; set; }
        public List<int> OfferIds { get; set; }
    }
}
