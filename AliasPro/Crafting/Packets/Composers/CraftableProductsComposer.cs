using AliasPro.API.Crafting.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Crafting.Packets.Composers
{
    public class CraftableProductsComposer : IMessageComposer
    {
        private readonly ICollection<ICraftingRecipe> _recipes;
        private readonly ICollection<ICraftingIngredient> _ingredients;

        public CraftableProductsComposer(ICollection<ICraftingRecipe> recipes, ICollection<ICraftingIngredient> ingredients)
        {
            _recipes = recipes;
            _ingredients = ingredients;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.CraftableProductsMessageComposer);

            message.WriteInt(_recipes.Count);
            foreach (ICraftingRecipe recipe in _recipes)
            {
                message.WriteString(recipe.Name);
                message.WriteString(recipe.Name); //??
            }

            message.WriteInt(_ingredients.Count);
            foreach (ICraftingIngredient ingredient in _ingredients)
            {
                message.WriteString(ingredient.Name);
            }

            return message;
        }
    }
}
