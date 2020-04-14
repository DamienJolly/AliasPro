using AliasPro.API.Catalog;
using AliasPro.API.Chat.Commands;
using AliasPro.API.Items;
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
        private readonly IItemController _itemController;

        public UpdateCommand(
            ICatalogController catalogController,
            IItemController itemController)
        {
            _catalogController = catalogController;
            _itemController = itemController;
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
                        return true;
                    }
                case "item":
                case "items":
                    {
                        _itemController.InitializeItems();
                        return true;
                    }
            }

            return false;
        }
    }
}
