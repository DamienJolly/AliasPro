using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Pets.Models;
using AliasPro.Pets.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Pets
{
    internal class PetDao : BaseDao
    {
        public PetDao(IConfigurationController configurationController)
            : base(configurationController)
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

						if (!pets.ContainsKey(pet.Type))
							pets.Add(pet.Type, pet);
					}
				}, "SELECT * FROM `pet_data`;");
			});
			return pets;
		}
	}
}
