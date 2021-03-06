﻿using AliasPro.API.Catalog.Layouts;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Database;
using AliasPro.Catalog.Layouts;
using System.Collections.Generic;
using System.Data.Common;

namespace AliasPro.Catalog.Models
{
    internal class CatalogPage : ICatalogPage
    {
        internal CatalogPage(DbDataReader reader)
        {
            Id = reader.ReadData<int>("id");
            ParentId = reader.ReadData<int>("parent_id");
            Name = reader.ReadData<string>("name");
            Caption = reader.ReadData<string>("caption");
            Icon = reader.ReadData<int>("icon");
            Rank = reader.ReadData<int>("rank");
            Order = reader.ReadData<int>("order_num");
            Items = new Dictionary<int, ICatalogItem>();
			HeaderImage = reader.ReadData<string>("header_image");
            TeaserImage = reader.ReadData<string>("teaser_image");
            SpecialImage = reader.ReadData<string>("special_image");
            TextOne = reader.ReadData<string>("text_one");
            TextTwo = reader.ReadData<string>("text_two");
            TextDetails = reader.ReadData<string>("text_details");
            TextTeaser = reader.ReadData<string>("text_teaser");
            Layout = GetCatalogLayout(reader.ReadData<string>("layout"));
            Enabled = reader.ReadData<bool>("enabled");
            Visible = reader.ReadData<bool>("visible");
        }

        private ICatalogLayout GetCatalogLayout(string layout)
        {
            switch (layout)
            {
                default: return new LayoutDefault(this);
                case "frontpage": return new LayoutFrontpage(this);
				case "guilds": return new LayoutGroup(this);
				case "badge_display": return new LayoutBadgeDisplay(this);
				case "trophies": return new LayoutTrophies(this);
				case "bot": return new LayoutBot(this);
				case "pets": return new LayoutPets(this);
				case "pets3": return new LayoutPets3(this);
				case "roomads": return new LayoutRoomAds(this);
				case "spaces_new": return new LayoutSpacesNew(this);
				case "recycler": return new LayoutRecycler(this);
				case "recycler_prizes": return new LayoutRecyclerPrizes(this);
				case "recycler_info": return new LayoutRecyclerInfo(this);
				case "marketplace": return new LayoutMarketplace(this);
				case "marketplace_own_items": return new LayoutMarketplaceOwnItems(this);
				case "default_3x3_color_grouping": return new LayoutColorGrouping(this);
				case "soundmachine": return new LayoutTrax(this);
			}
        }

        public bool TryGetCatalogItem(int itemId, out ICatalogItem item) =>
            Items.TryGetValue(itemId, out item);

		public int Id { get; }
        public int ParentId { get; }
        public string Name { get; }
        public string Caption { get; }
        public int Icon { get; }
        public int Rank { get; }
        public int Order { get; }
        public IDictionary<int, ICatalogItem> Items { get; set; }
		public string HeaderImage { get; }
        public string TeaserImage { get; }
        public string SpecialImage { get; }
        public string TextOne { get; }
        public string TextTwo { get; }
        public string TextDetails { get; }
        public string TextTeaser { get; }
        public ICatalogLayout Layout { get; }
        public bool Enabled { get; }
        public bool Visible { get; }
    }
}
