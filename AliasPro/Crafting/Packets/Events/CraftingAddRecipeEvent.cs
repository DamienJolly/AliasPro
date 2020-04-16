using AliasPro.API.Crafting;
using AliasPro.API.Crafting.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Crafting.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Crafting.Packets.Events
{
	public class CraftingAddRecipeEvent : IMessageEvent
	{
		public short Header => Incoming.CraftingAddRecipeMessageEvent;

		private readonly ICraftingController _craftingController;

		public CraftingAddRecipeEvent(
			ICraftingController craftingController)
		{
			_craftingController = craftingController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage message)
		{
			IRoom room = session.CurrentRoom;
			if (room == null)
				return;

			string recipeName = message.ReadString();
			if (!_craftingController.TryGetRecipe(recipeName, out ICraftingRecipe recipe))
				return;

			if (!recipe.CanBeCrafted)
			{
				await session.SendPacketAsync(new AlertLimitedSoldOutComposer());
				return;
			}

			await session.SendPacketAsync(new CraftingRecipeComposer(recipe));
		}
	}
}
