using AliasPro.API.Catalog;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Sessions;
using System.Threading.Tasks;

namespace AliasPro.Chat.Models.Commands
{
    internal class UpdateCommand : IChatCommand
    {
        public string Name => "update";
        public string Description => "Updates shit.";

        private readonly ICatalogController _catalogController;

        public UpdateCommand(ICatalogController catalogController)
        {
            _catalogController = catalogController;
        }

        public async Task Handle(ISession session, string[] args)
        {
            if (args.Length >= 1)
            {
                string query = args[0];
                switch (query)
                {
                    case "cata":
                    case "catalog":
                    case "catalogue":
                        {
                            _catalogController.ReloadCatalog();
                            await session.SendPacketAsync(new CatalogUpdatedComposer());
                            break;
                        }
                }
            }
        }
    }
}
