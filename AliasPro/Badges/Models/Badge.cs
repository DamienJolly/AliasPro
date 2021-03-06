﻿using AliasPro.API.Badges.Models;
using AliasPro.API.Database;
using System.Data.Common;

namespace AliasPro.Badges.Models
{
	internal class Badge : IBadge
	{
		internal Badge(DbDataReader reader)
		{
			Id = reader.ReadData<int>("id");
			Code = reader.ReadData<string>("code");
			RequiredRight = reader.ReadData<string>("required_right");
		}

		public int Id { get; set; }
		public string Code { get; set; }
		public string RequiredRight { get; set; }
	}
}
