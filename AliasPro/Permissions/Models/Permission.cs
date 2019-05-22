using AliasPro.API.Database;
using AliasPro.API.Permissions.Models;
using System.Collections.Generic;
using System.Data.Common;

namespace AliasPro.Permissions.Models
{
	internal class Permission : IPermission
	{
		internal Permission(DbDataReader reader)
		{
			Level = reader.ReadData<int>("level");
			Permissions = new List<string>();
		}

		public bool HasPermission(string param) => 
			Permissions.Contains(param);

		public int Level { get; set; }
		public IList<string> Permissions { get; set; }
	}
}
