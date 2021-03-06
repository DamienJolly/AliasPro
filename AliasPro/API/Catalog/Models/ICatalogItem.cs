﻿using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.API.Catalog.Models
{
    public interface ICatalogItem
    {
        int Id { get; }
        int PageId { get; }
        IList<ICatalogItemData> Items { get; }
		string Name { get; }
        int Credits { get; }
        int Points { get; }
        int PointsType { get; }
        int ClubLevel { get; }
        bool CanGift { get; }
        bool HasOffer { get; }
        int OfferId { get; }
        int LimitedStack { get; }
        IList<int> LimitedNumbers { get; set; }
        int LimitedSells { get; }
        bool IsLimited { get; }
        bool TryGetLimitedNumber(out int limitedNumber);

        void Compose(ServerMessage message);
    }
}
