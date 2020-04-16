using AliasPro.API.Crafting.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Crafting.Packets.Composers
{
    public class CraftingRecipeComposer : IMessageComposer
    {
        private readonly ICraftingRecipe _recipe;

        public CraftingRecipeComposer(ICraftingRecipe recipe)
        {
            _recipe = recipe;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.CraftingRecipeMessageComposer);

            message.WriteInt(_recipe.Ingredients.Count);
            foreach (ICraftingIngredient ingredient in _recipe.Ingredients.Values)
            {
                message.WriteInt(ingredient.Amount);
                message.WriteString(ingredient.Name);
            }

            return message;
        }
    }
}
