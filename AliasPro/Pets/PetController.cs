using AliasPro.API.Pets;
using AliasPro.API.Pets.Models;
using System.Collections.Generic;

namespace AliasPro.Pets
{
    internal class PetController : IPetController
	{
		private readonly PetDao _petDao;

		private IDictionary<int, IPetData> _petData;

		public PetController(PetDao petDao)
        {
			_petDao = petDao;

			_petData = new Dictionary<int, IPetData>();

			InitializePets();
		}

		public async void InitializePets()
		{
			_petData = await _petDao.ReadPetData();

			System.Console.WriteLine(_petData.Count);
		}

		public bool TryGetPetData(int type, out IPetData pet) =>
			_petData.TryGetValue(type, out pet);

		public bool TryGetPetData(string name, out IPetData pet)
		{
			int type = int.Parse(name.Replace("a0 pet", ""));
			return TryGetPetData(type, out pet);
		}
	}
}
