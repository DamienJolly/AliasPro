using AliasPro.API.Catalog.Models;
using AliasPro.API.Database;
using AliasPro.Players.Types;
using System.Data.Common;

namespace AliasPro.Catalog.Models
{
    internal class CatalogBot : ICatalogBot
	{
        internal CatalogBot(DbDataReader reader)
        {
			ItemId = reader.ReadData<int>("item_id");
            Name = reader.ReadData<string>("name");
            Motto = reader.ReadData<string>("motto");
            Figure = reader.ReadData<string>("figure");
            Gender = reader.ReadData<string>("gender") == "m" ? PlayerGender.MALE : PlayerGender.FEMALE;
        }

        public int ItemId { get; }
        public string Name { get; }
        public string Motto { get; }
        public string Figure { get; }
        public PlayerGender Gender { get; }
    }
}
