using AliasPro.Players.Types;

namespace AliasPro.Game.Catalog.Models
{
    public class CatalogBotData
    {
        public CatalogBotData(int itemId, string name, string motto, string figure, string gender)
        {
            ItemId = itemId;
            Name = name;
            Motto = motto;
            Figure = figure;
            //todo: gender utility
            Gender = gender.ToLower() == "m" ? PlayerGender.MALE : PlayerGender.FEMALE;
        }

        public int ItemId { get; }
        public string Name { get; }
        public string Motto { get; }
        public string Figure { get; }
        public PlayerGender Gender { get; }
    }
}
