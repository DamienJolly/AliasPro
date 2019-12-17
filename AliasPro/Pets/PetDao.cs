using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Pets.Models;
using AliasPro.Pets.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Pets
{
    internal class PetDao : BaseDao
    {
        public PetDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
			: base(logger, configurationController)
		{

		}

		public async Task<IDictionary<int, IPetData>> ReadPetData()
		{
			IDictionary<int, IPetData> pets = new Dictionary<int, IPetData>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						IPetData pet = new PetData(reader);
						pet.Breeds = await ReadPetBreeds(pet.Type);

						if (!pets.ContainsKey(pet.Type))
							pets.Add(pet.Type, pet);
					}
				}, "SELECT * FROM `pet_data`;");
			});
			return pets;
		}

		public async Task<IList<IPetBreed>> ReadPetBreeds(int type)
		{
			IList<IPetBreed> breeds = new List<IPetBreed>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						breeds.Add(new PetBreed(reader));
					}
				}, "SELECT * FROM `pet_breeds` WHERE `race` = @0;", type);
			});
			return breeds;
		}
	}
}
