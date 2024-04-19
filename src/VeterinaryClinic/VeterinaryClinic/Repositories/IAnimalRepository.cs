using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories;

public interface IAnimalRepository
{
    IEnumerable<Animal> GetAll(string orderBy);
    Animal? GetById(int id);
    bool AddAnimal(Animal newAnimal);
    int UpdateAnimal(Animal animal);
    bool DeleteAnimal(int id);

}