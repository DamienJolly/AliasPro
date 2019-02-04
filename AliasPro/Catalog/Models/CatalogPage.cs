using System.Data.Common;

namespace AliasPro.Catalog.Models
{
    using Database;

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
			HeaderImage = reader.ReadData<string>("header_image");
			TeaserImage = reader.ReadData<string>("teaser_image");
			SpecialImage = reader.ReadData<string>("special_image");
			TextOne = reader.ReadData<string>("text_one");
			TextTwo = reader.ReadData<string>("text_two");
			TextDetails = reader.ReadData<string>("text_details");
			TextTeaser = reader.ReadData<string>("text_teaser");
            Layout = reader.ReadData<string>("layout");
			Enabled = reader.ReadData<bool>("enabled");
			Visible = reader.ReadData<bool>("visible");
        }

        public int Id { get; }
        public int ParentId { get; }
        public string Name { get; }
        public string Caption { get; }
        public int Icon { get; }
        public int Rank { get; }
        public int Order { get; }
        public string HeaderImage { get; }
        public string TeaserImage { get; }
        public string SpecialImage { get; }
        public string TextOne { get; }
        public string TextTwo { get; }
        public string TextDetails { get; }
        public string TextTeaser { get; }
        public string Layout { get; }
        public bool Enabled { get; }
        public bool Visible { get; }
    }

    public interface ICatalogPage
    {
        int Id { get; }
        int ParentId { get; }
        string Name { get; }
        string Caption { get; }
        int Icon { get; }
        int Rank { get; }
        int Order { get; }
        string HeaderImage { get; }
        string TeaserImage { get; }
        string SpecialImage { get; }
        string TextOne { get; }
        string TextTwo { get; }
        string TextDetails { get; }
        string TextTeaser { get; }
        string Layout { get; }
        bool Enabled { get; }
        bool Visible { get; }
    }
}
