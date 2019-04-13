using System.Threading.Tasks;

namespace AliasPro.API.Settings
{
    public interface ISettingsController
    {
        string GetSetting(string key);
        Task CleanupDatabase();
    }
}
