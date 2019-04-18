﻿using AliasPro.API.Moderation;
using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Moderation
{
    internal class ModerationService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<ModerationDao>();
            collection.AddSingleton<IModerationController, ModerationController>();

            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {

        }
    }
}