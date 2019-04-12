using AliasPro.API.Chat.Commands;
using AliasPro.Chat.Components;
using AliasPro.Sessions;
using System.Collections.Generic;

namespace AliasPro.Chat
{
    internal class ChatRepostiory
    {
        private readonly CommandComponent _commandComponent;

        public ChatRepostiory(IEnumerable<IChatCommand> commands)
        {
            _commandComponent = new CommandComponent(commands);
        }

        public bool HandleCommand(ISession session, string message) =>
            _commandComponent.Handle(session, message);
    }
}
