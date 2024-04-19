using Microsoft.Data.SqlClient;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories;

public class AnimalRepository(string connectionString) : IAnimalRepository
{
    public IEnumerable<Animal> GetAll(string orderBy)
    {
        List<Animal> animals = new();
        const string queryString = """
                                   SELECT * 
                                   FROM Animal
                                   ORDER BY 
                                       CASE WHEN @orderBy='IdAnimal' THEN IdAnimal END,
                                       CASE WHEN @orderBy='Name' THEN Name END,
                                       CASE WHEN @orderBy='Description' THEN Description END,
                                       CASE WHEN @orderBy='Category' THEN Category END,
                                       CASE WHEN @orderBy='Area' THEN Area END
                                   """;
        using (SqlConnection connection = new(connectionString))
        {
            using (SqlCommand command = new(queryString, connection))
            {
                command.Parameters.AddWithValue("@orderBy", orderBy);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var animal = new Animal
                            {
                                IdAnimal = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Category = reader.GetString(3),
                                Area = reader.GetString(4),
                            };
                            animals.Add(animal);
                        }
                    }
                }
            }
        }

        return animals;
    }

    public Animal? GetById(int id)
    {
        Animal? specificAnimal = null;
        const string queryString = "SELECT * FROM Animal WHERE IdAnimal = @IdAnimal";
        using (SqlConnection connection = new(connectionString))
        {
            using (SqlCommand command = new(queryString, connection))
            {
                command.Parameters.AddWithValue("IdAnimal", id);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            specificAnimal = new Animal
                            {
                                IdAnimal = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Category = reader.GetString(3),
                                Area = reader.GetString(4),
                            };
                        }
                    }
                }
            }
        }

        return specificAnimal;
    }
    
    public bool AddAnimal(Animal newAnimal) 
    {
        const string insertString = "INSERT INTO Animal(IdAnimal, Name, Description, Category, Area) VALUES (@IdAnimal, @Name, @Description, @Category, @Area)";
        int countRowsAdded = -1;
        using (SqlConnection connection = new SqlConnection(connectionString)) 
        {
            using (SqlCommand command = new(insertString, connection))
            {
                command.Parameters.AddWithValue("IdAnimal", newAnimal.IdAnimal);
                command.Parameters.AddWithValue("Name", newAnimal.Name);
                if (newAnimal.Description == null)
                {
                    command.Parameters.AddWithValue("Description", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("Description", newAnimal.Description);
                }
                command.Parameters.AddWithValue("Category", newAnimal.Category);
                command.Parameters.AddWithValue("Area", newAnimal.Area);

                connection.Open();
                countRowsAdded = command.ExecuteNonQuery();
            }
        }
    
        return countRowsAdded != -1;
    }

    public bool DeleteAnimal(int id)
    {
        const string deleteString = "DELETE FROM Animal WHERE IdAnimal = @IdAnimal";
        int countRowsDeleted = -1;
        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand(deleteString, connection))
            {
                command.Parameters.AddWithValue("IdAnimal", id);
                connection.Open();
                countRowsDeleted = command.ExecuteNonQuery();
            }
        }
        return countRowsDeleted != -1;
    }

    public int UpdateAnimal(Animal animal)
    {
        const string updateString = "UPDATE Animal SET Name=@Name, Description=@Description, Category=@Category, Area=@Area WHERE IdAnimal = @IdAnimal";
        var countRowsUpdated = -1;
        using (SqlConnection connection = new SqlConnection(connectionString)) 
        {
            using (SqlCommand command = new(updateString, connection))
            {
                command.Parameters.AddWithValue("IdAnimal", animal.IdAnimal);
                command.Parameters.AddWithValue("Name", animal.Name);
                if (animal.Description == null)
                {
                    command.Parameters.AddWithValue("Description", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("Description", animal.Description);
                }
                command.Parameters.AddWithValue("Category", animal.Category);
                command.Parameters.AddWithValue("Area", animal.Area);

                connection.Open();
                countRowsUpdated = command.ExecuteNonQuery();
            }
        }
        return countRowsUpdated;
    }
}