using AliasPro.API.Players.Models;

namespace AliasPro.API.Permissions
{
    public interface IPermissionsController
	{
		void InitializePermissions();
		bool HasPermission(IPlayer player, string param);
	}
}
