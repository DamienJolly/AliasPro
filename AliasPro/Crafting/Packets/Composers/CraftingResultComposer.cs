using AliasPro.API.Crafting.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Crafting.Packets.Composers
{
    public class CraftingResultComposer : IMessageComposer
    {
        private readonly bool _success;
        private readonly ICraftingRecipe _recipe;

        public CraftingResultComposer(ICraftingRecipe recipe, bool success)
        {
            _recipe = recipe;
            _success = success;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.CraftingResultMessageComposer);

            message.WriteBoolean(_success);

            if (_success)
            {
                message.WriteString(_recipe.Name);
                message.WriteString(_recipe.Name); //??
            }

            return message;
        }
    }
}
