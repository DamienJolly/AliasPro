using AliasPro.API.Pets.Models;

namespace AliasPro.API.Pets
{
    public interface IPetController
	{
		void InitializePets();

		bool TryGetPetData(int type, out IPetData pet);
		bool TryGetPetData(string name, out IPetData pet);
	}
}
