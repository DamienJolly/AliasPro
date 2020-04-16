using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Crafting.Packets.Composers
{
    public class CraftingRecipesAvailableComposer : IMessageComposer
    {
        private readonly int _count;
        private readonly bool _found;

        public CraftingRecipesAvailableComposer(int count, bool found)
        {
            _count = count;
            _found = found;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.CraftingRecipesAvailableMessageComposer);
            message.WriteInt((_found ? -1 : 0) + _count);
            message.WriteBoolean(_found);
            return message;
        }
    }
}
