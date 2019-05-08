using System.Threading.Tasks;

namespace AliasPro.API.Server
{
    public interface IServerController
    {
        void LoadEmulatorSettings();
        string GetSetting(string key);
        Task CleanupDatabase();
    }
}
