using AliasPro.API.Groups.Models;
using AliasPro.API.Database;
using System.Data.Common;
using AliasPro.Groups.Utilities;
using AliasPro.Groups.Types;

namespace AliasPro.Groups.Models
{
	internal class GroupBadgePart : IGroupBadgePart
	{
		internal GroupBadgePart(DbDataReader reader)
		{
			Id = reader.ReadData<int>("id");
			AssetOne = reader.ReadData<string>("code");
			AssetTwo = reader.ReadData<string>("code2");
			Type = BadgePartUtility.GetBadgePartType(
				reader.ReadData<string>("type"));
		}

		public int Id { get; set; }
		public string AssetOne { get; set; }
		public string AssetTwo { get; set; }
		public BadgePartType Type { get; set; }
	}
}
