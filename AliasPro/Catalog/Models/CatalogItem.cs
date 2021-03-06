﻿using AliasPro.API.Catalog.Models;
using AliasPro.API.Database;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Types;
using System.Collections.Generic;
using System.Data.Common;

namespace AliasPro.Catalog.Models
{
    internal class CatalogItem : ICatalogItem
    {
        internal CatalogItem(DbDataReader reader)
        {
            Id = reader.ReadData<int>("id");
            PageId = reader.ReadData<int>("page_Id");
            Items = new List<ICatalogItemData>();
            Name = reader.ReadData<string>("catalog_name");
            Credits = reader.ReadData<int>("cost_credits");
            Points = reader.ReadData<int>("cost_points");
            PointsType = reader.ReadData<int>("points_type");
            ClubLevel = reader.ReadData<int>("club_level");
            CanGift = reader.ReadData<bool>("can_gift");
            HasOffer = reader.ReadData<bool>("have_offer");
            OfferId = reader.ReadData<int>("offer_id");
            LimitedStack = reader.ReadData<int>("limited_stack");
            LimitedNumbers = new List<int>();
        }

        public int Id { get; }
        public int PageId { get; }
        public IList<ICatalogItemData> Items { get; }
        public string Name { get; }
        public int Credits { get; }
        public int Points { get; }
        public int PointsType { get; }
        public int ClubLevel { get; }
        public bool CanGift { get; }
        public bool HasOffer { get; }
        public int OfferId { get; }
        public int LimitedStack { get; }
        public IList<int> LimitedNumbers { get; set; }

        public int LimitedSells
        {
            get
            {
                return LimitedStack - LimitedNumbers.Count;
            }
        }

        public bool IsLimited
        {
            get
            {
                return LimitedStack > 0;
            }
        }

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

			message.WriteInt(Items.Count);
			foreach (ICatalogItemData item in Items)
			{
				message.WriteString(item.ItemData.Type);
				if (item.ItemData.Type == "b")
				{
					message.WriteString(item.ItemData.ExtraData);
				}
				else
				{
					message.WriteInt((int)item.ItemData.SpriteId);

                    if (item.ItemData.InteractionType == ItemInteractionType.WALLPAPER ||
                        item.ItemData.InteractionType == ItemInteractionType.FLOOR ||
                        item.ItemData.InteractionType == ItemInteractionType.LANDSCAPE)
                    {
                        message.WriteString(Name.Split('_')[2]);
                    }
                    else
                    {
                        message.WriteString(item.BotData != null ? item.BotData.Figure : item.ItemData.ExtraData);
                    }

					message.WriteInt(item.Amount);

					message.WriteBoolean(IsLimited && item.Amount <= 1);
					if (IsLimited && item.Amount <= 1)
					{
						message.WriteInt(LimitedStack);
						message.WriteInt(LimitedStack - LimitedSells);
					}
				}
			}

			message.WriteInt(ClubLevel);
            message.WriteBoolean(HasOffer);
            message.WriteBoolean(false);
            message.WriteString(Name + ".png");
        }
    }
}
