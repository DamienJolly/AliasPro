using System.Collections.Generic;

namespace AliasPro.API.Permissions.Models
{
	public interface IPermission
	{
		bool HasPermission(string param);

		int Level { get; set; }
		IList<string> Permissions { get; set; }
	}
}
