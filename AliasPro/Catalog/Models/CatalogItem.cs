﻿using System.Data.Common;
using System.Collections.Generic;

namespace AliasPro.Catalog.Models
{
    using Item;
    using Item.Models;
    using Database;
    using Network.Protocol;

    internal class CatalogItem : ICatalogItem
    {
        internal CatalogItem(DbDataReader reader, ItemRepository itemRepository)
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
            
            string itemData = reader.ReadData<string>("item_ids");
            foreach(string items in itemData.Split(':'))
            {
                string[] data = items.Split('*');
                int amount = 1;

                if (itemRepository.TryGetItemDataById(uint.Parse(data[0]), out IItemData item))
                {
                    if (data.Length >= 2)
                        amount = int.Parse(data[1]);

                    Items.Add(new CatalogItemData((int)item.Id, amount, item));
                }
            }
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

        public int GetNumber
        {
            get
            {
                return LimitedNumbers[0];
            }
        }

        public void Compose(ServerPacket message)
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
                message.WriteInt(item.ItemData.SpriteId);
                message.WriteString(item.ItemData.ExtraData);
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
        int GetNumber { get; }

        void Compose(ServerPacket message);
    }
}