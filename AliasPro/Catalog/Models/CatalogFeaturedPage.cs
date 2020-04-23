using AliasPro.API.Catalog.Models;
using AliasPro.API.Database;
using AliasPro.Catalog.Types;
using AliasPro.Utilities;
using System;
using System.Data.Common;

namespace AliasPro.Catalog.Models
{
    internal class CatalogFeaturedPage : ICatalogFeaturedPage
    {
        internal CatalogFeaturedPage(DbDataReader reader)
        {
            SlotId = reader.ReadData<int>("slot_id");
            Caption = reader.ReadData<string>("caption");
            Image = reader.ReadData<string>("image");
            Type = reader.ReadData<string>("type").ToUpper().ToEnum<FeaturedPageType>();
            PageName = reader.ReadData<string>("page_name");
            PageId = reader.ReadData<int>("page_id");
            ProductName = reader.ReadData<string>("product_name");
            ExpireTimestamp = reader.ReadData<int>("expire_timestamp");
        }

		public int SlotId { get; }
        public string Caption { get; }
        public string Image { get; }
        public FeaturedPageType Type { get; }
        public string PageName { get; }
        public int PageId { get; }
        public string ProductName { get; }
        public int ExpireTimestamp { get; }
    }
}
