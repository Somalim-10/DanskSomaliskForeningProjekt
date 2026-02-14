using Microsoft.AspNetCore.Mvc;
using SomaliskDanskForening_API.DTO_S;
using SomaliskDanskForening_Lib.Interfaces;
using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepo _contactRepo;

        public ContactController(IContactRepo contactRepo)
        {
            _contactRepo = contactRepo;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var contacts = _contactRepo.GetAll();
            if (contacts == null || contacts.Count == 0)
                return NotFound("No contacts found.");
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Contact> GetById(int id)
        {
            var c = _contactRepo.GetById(id);
            if (c == null) return NotFound($"Contact with ID {id} not found.");
            return Ok(c);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Contact> Post([FromBody] ContactDTO contact)
        {
            try
            {
                var added = _contactRepo.Add(contact.ToContact());
                return CreatedAtAction(nameof(GetById), new { id = added.Id }, added);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Contact> Put(int id, [FromBody] ContactDTO contact)
        {
            try
            {
                var c = contact.ToContact();
                c.Id = id;
                var updated = _contactRepo.Update(c);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Contact> Delete(int id)
        {
            var existing = _contactRepo.GetById(id);
            if (existing == null) return NotFound($"Contact with ID {id} not found.");
            _contactRepo.Delete(id);
            return Ok(existing);
        }
    }
}
