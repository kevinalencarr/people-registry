using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PeopleRegistry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleRegistryController : ControllerBase
    {
        private readonly DataContext _context;

        public PeopleRegistryController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get()
        {
            return Ok(await _context.People.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
                return BadRequest("Person not found.");
            return Ok(person);
        }

        [HttpPost]
        public async Task<ActionResult<List<Person>>> Add(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return Ok(await _context.People.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Person>>> Update(Person request)
        {
            var dbPerson = await _context.People.FindAsync(request.Id);
            if (dbPerson == null)
                return BadRequest("Person not found.");

            dbPerson.FirstName = request.FirstName;
            dbPerson.LastName = request.LastName;
            dbPerson.Email = request.Email;

            await _context.SaveChangesAsync();

            return Ok(await _context.People.ToListAsync());
        }

        [HttpDelete]
        public async Task<ActionResult<List<Person>>> Delete(int id)
        {
            var dbPerson = await _context.People.FindAsync(id);
            if (dbPerson == null)
                return BadRequest("Person not found.");

            _context.People.Remove(dbPerson);
            await _context.SaveChangesAsync();
            return Ok(await _context.People.ToListAsync());
        }
    }
}
