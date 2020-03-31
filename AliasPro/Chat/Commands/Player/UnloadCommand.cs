﻿using AliasPro.API.Chat.Commands;
using AliasPro.API.Rooms;
using AliasPro.API.Sessions.Models;
using System.Threading.Tasks;

namespace AliasPro.Chat.Commands
{
    internal class UnloadCommand : IChatCommand
    {
        public string[] Names => new[]
        {
            "unload"
        };

        public string PermissionRequired => "cmd_unload";

        public string Parameters => "";

        public string Description => "Unloads the current room you are in.";

        private readonly IRoomController _roomController;

        public UnloadCommand(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public Task<bool> Handle(ISession session, string[] args)
        {
            if (session.CurrentRoom == null)
                return Task.FromResult(false);

            if (!session.CurrentRoom.Rights.IsOwner(session.Player.Id))
                return Task.FromResult(false);

            _roomController.DisposeRoom(session.CurrentRoom);
            return Task.FromResult(true);
        }
    }
}
