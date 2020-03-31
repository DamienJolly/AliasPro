using AliasPro.API.Catalog;
using AliasPro.API.Chat.Commands;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Chat.Commands
{
    internal class UpdateCommand : IChatCommand
    {
        public string[] Names => new[]
        {
            "update"
        };

        public string PermissionRequired => "cmd_update";

        public string Parameters => "%query%";

        public string Description => "Updates shit.";

        private readonly ICatalogController _catalogController;

        public UpdateCommand(ICatalogController catalogController)
        {
            _catalogController = catalogController;
        }

        public async Task<bool> Handle(ISession session, string[] args)
        {
            if (args.Length <= 0)
                return false;

            string query = args[0];
            switch (query)
            {
                case "cata":
                case "catalog":
                case "catalogue":
                    {
                        _catalogController.InitializeCatalog();
                        await session.SendPacketAsync(new CatalogUpdatedComposer());
                        break;
                    }
            }

            return true;
        }
    }
}
