﻿using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Chat
{
    using Network;
    using Models.Commands;

    internal class ChatService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<ChatDao>();
            collection.AddSingleton<ChatRepostiory>();
            collection.AddSingleton<IChatController, ChatController>();

            AddPackets(collection);
            AddCommands(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {

        }

        private static void AddCommands(IServiceCollection collection)
        {
            collection.AddSingleton<IChatCommand, UpdateCommand>();
        }
    }
}
