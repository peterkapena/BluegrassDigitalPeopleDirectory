using BluegrassDigitalPeopleDirectory.Models;
using BluegrassDigitalPeopleDirectory.Services;
using BluegrassDigitalPeopleDirectory.Services.Bug;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BluegrassDigitalPeopleDirectory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PeopleController(UserManager<User> userMgr, IErrorLogService errorLogService, IPeopleService peopleService) :
        CommonAPI(userMgr: userMgr, errorLogService: errorLogService)
    {
        public IPeopleService PeopleService { get; } = peopleService;

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return Ok(await PeopleService.GetPeople());
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            if (!PeopleService.PersonExists(id))
            {
                return NotFound();
            }

            return Ok(await PeopleService.GetPerson(id));
        }

        // PUT: api/People/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(long id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            await PeopleService.UpdatePerson(id, person);

            return NoContent();
        }

        // POST: api/People
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            await PeopleService.AddPerson(person);

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(long id)
        {

            if (!PeopleService.PersonExists(id))
            {
                return NotFound();
            }

            await PeopleService.DeletePerson(id);

            return NoContent();
        }
    }
}
