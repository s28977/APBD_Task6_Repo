using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;
using Microsoft.IdentityModel.Tokens;


[ApiController]
public class AnimalController(IAnimalRepository animalRepository) : ControllerBase
{
    [HttpGet("api/animals")]
    public IActionResult Get()
    {
        var orderBy = HttpContext.Request.Query["orderBy"];
        if (orderBy.IsNullOrEmpty())
        {
            return Ok(animalRepository.GetAll("Name"));
        }
        return Ok(animalRepository.GetAll(orderBy));
    }

    [HttpGet("api/animals/{id:int}")]
    public IActionResult Get(int id)
    {
        var animal = animalRepository.GetById(id);
        return animal == null ? NotFound("Animal with ID " + id + " is not found") : Ok(animalRepository.GetById(id));
    }

    [HttpPost("api/animals")]
    public IActionResult Create([FromBody] Animal animal)
    {
        var isCreated = animalRepository.AddAnimal(animal);
        return isCreated ? StatusCode(StatusCodes.Status201Created) : BadRequest();
    }

    [HttpPut("api/animals/{id:int}")]
    public IActionResult Update(int id, [FromBody] Animal animal)
    {
        if (id != animal.IdAnimal)
        {
            return BadRequest();
        }
        var isUpdated = animalRepository.UpdateAnimal(animal);
        return isUpdated switch
        {
            0 => Create(animal),
            1 => NoContent(),
            _ => BadRequest()
        };
    }

    [HttpDelete("api/animals/{id:int}")]
    public IActionResult Delete(int id)
    {
        var isDeleted = animalRepository.DeleteAnimal(id);
        return isDeleted ? NoContent() : BadRequest();
    }
}