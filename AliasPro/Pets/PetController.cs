using AliasPro.API.Pets;
using AliasPro.API.Pets.Models;
using AliasPro.Utilities;
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
		}

		public bool CheckPetName(string petName)
		{
			int minLength = 2; //add to config
			int maxLength = 15;

			if (petName.Length < minLength)
				return false;

			if (petName.Length > maxLength)
				return false;

			if (!StringUtils.IsAlphanumeric(petName))
				return false;

			return true;
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
