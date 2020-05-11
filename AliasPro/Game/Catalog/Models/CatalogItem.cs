using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Types;
using System.Collections.Generic;

namespace AliasPro.Game.Catalog.Models
{
	public class CatalogItem
	{
        public int Id { get; }
        public int PageId { get; }
        public string Name { get; }
        public int Credits { get; }
        public int Points { get; }
        public int PointsType { get; }
        public int ClubLevel { get; }
        public bool CanGift { get; }
        public bool HasOffer { get; }
        public int OfferId { get; }
        public int BotId { get; }
        public string Badge { get; set; }
        public int LimitedStack { get; }
        public IList<CatalogItemData> Items { get; }
        public IList<int> LimitedNumbers { get; set; }

        public CatalogItem(int id, int pageId, string name, int credits, int points, int pointsType, int clubLevel, bool canGift, bool hasOffer, int offerId, int botId, string badge, int limitedStack)
        {
            Id = id;
            PageId = pageId;
            Name = name;
            Credits = credits;
            Points = points;
            PointsType = pointsType;
            ClubLevel = clubLevel;
            CanGift = canGift;
            HasOffer = hasOffer;
            OfferId = offerId;
            BotId = botId;
            Badge = badge;
            LimitedStack = limitedStack;
            Items = new List<CatalogItemData>();
            LimitedNumbers = new List<int>();
        }

        public int LimitedSells =>
            LimitedStack - LimitedNumbers.Count;

        public bool IsLimited =>
            LimitedStack > 0;

        public bool hasBadge =>
            !string.IsNullOrEmpty(Badge);

        public bool TryGetLimitedNumber(out int limitedNumber)
        {
            if (IsLimited && LimitedNumbers.Count > 0)
            {
                limitedNumber = LimitedNumbers[0];
                LimitedNumbers.Remove(limitedNumber);
                return true;
            }

            limitedNumber = -1;
            return false;
        }

        public void Compose(ServerMessage message)
        {
            message.WriteInt(Id);
            message.WriteString(Name);
            message.WriteBoolean(false);
            message.WriteInt(Credits);
            message.WriteInt(Points);
            message.WriteInt(PointsType);
            message.WriteBoolean(CanGift);

            if (!hasBadge)
            {
                message.WriteInt(Items.Count);
            }
            else
            {
                message.WriteInt(Items.Count + 1);
                message.WriteString("b");
                message.WriteString(Badge);
            }

            foreach (CatalogItemData item in Items)
            {
                message.WriteString(item.ItemData.Type);
                message.WriteInt((int)item.ItemData.SpriteId);
                message.WriteString(item.Extradata);
                message.WriteInt(item.Amount);

                message.WriteBoolean(IsLimited && item.Amount <= 1);
                if (IsLimited && item.Amount <= 1)
                {
                    message.WriteInt(LimitedStack);
                    message.WriteInt(LimitedStack - LimitedSells);
                }
            }

            message.WriteInt(ClubLevel);
            message.WriteBoolean(HasOffer);
            message.WriteBoolean(false);
            message.WriteString(Name + ".png");
        }
    }
}
