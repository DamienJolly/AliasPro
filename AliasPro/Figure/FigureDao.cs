﻿using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Figure.Models;
using AliasPro.Figure.Models;
using AliasPro.Players.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Figure
{
    internal class FigureDao : BaseDao
    {
        public FigureDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }

		internal async Task<IDictionary<int, IWardrobeItem>> GetPlayerWardrobeAsync(uint id)
		{
			IDictionary<int, IWardrobeItem> items = new Dictionary<int, IWardrobeItem>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						IWardrobeItem item = new WardrobeItem(reader);
						if (!items.TryAdd(item.SlotId, item))
						{
							// failed
						}
					}
				}, "SELECT * FROM `player_wardrobe` WHERE `player_id` = @0;", id);
			});
			return items;
		}

		internal async Task AddWardrobeItemAsync(uint playerId, IWardrobeItem item)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "INSERT INTO `player_wardrobe` (`player_id`, `slot_id`, `figure`, `gender`) VALUES (@0, @1, @2, @3);", 
					playerId, item.SlotId, item.Figure, item.Gender == PlayerGender.MALE ? "m" : "f");
			});
		}

		internal async Task UpdateWardrobeItemAsync(uint playerId, ICollection<IWardrobeItem> items)
		{
			await CreateTransaction(async transaction =>
			{
				foreach (IWardrobeItem item in items)
				{
					await Insert(transaction, "UPDATE `player_wardrobe` SET `figure` = @2, `gender` = @3 WHERE `player_id` = @0 AND `slot_id` = @1;",
						playerId, item.SlotId, item.Figure, item.Gender == PlayerGender.MALE ? "m" : "f");
				}
			});
		}
	}
}