﻿using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Catalog
{
    using Network;
    //using Network.Events;
    //using Packets.Incoming;

    internal class CatalogService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<CatalogDao>();
            collection.AddSingleton<CatalogRepostiory>();
            collection.AddSingleton<ICatalogController, CatalogController>();

            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {

        }
    }
}
