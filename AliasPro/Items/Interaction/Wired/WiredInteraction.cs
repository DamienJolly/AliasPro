﻿using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Tasks;

namespace AliasPro.Items.Interaction.Wired
{
    public abstract class WiredInteraction : ITask
    {
        public IItem Item;

        public IRoom Room =>
            Item.CurrentRoom;
        public IWiredData WiredData { get; set; }

        private object[] _args;

        public WiredInteraction(IItem item, int type)
        {
            Item = item;
            WiredData =
                new WiredData(type, Item.WiredData);
        }

        public virtual void ResetTimers() { }

        public virtual void Execute(params object[] args)
        {
            _args = args;
            Item.Interaction.OnUserInteract(null);
            Program.Tasks.ExecuteTask(this, RequiredCooldown);
        }

        public void Run()
        {
            if (!TryHandle(_args))
                return;
        }

        public abstract bool TryHandle(params object[] args);

        public virtual int RequiredCooldown => WiredData.Delay * 500;
    }
}
