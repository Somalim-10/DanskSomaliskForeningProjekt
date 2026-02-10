using Microsoft.AspNetCore.Mvc;
using SomaliskDanskForening_API.DTO_S;
using SomaliskDanskForening_Lib.Interfaces;
using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepo _eventRepo;

        public EventController(IEventRepo eventRepo)
        {
            _eventRepo = eventRepo;

        }




        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAll()
        {
            var events = _eventRepo.GetAll();
            if (events == null)
            {
                return NotFound("No events found.");
            }
                return Ok(events);


        }

        [HttpPost] 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Event> Post([FromBody] EventsDTO events)
        {
            try
            {

                var newEvent = _eventRepo.Add(events.ToEventDTO());
                return CreatedAtAction(nameof(GetById), new { id = newEvent.Id }, newEvent);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Event> Put(int id, [FromBody] EventsDTO events)
        {
            try
            {
                Event evt = events.ToEventDTO();
                evt.Id = id;
                var updatedEvent = _eventRepo.Update(id, evt);
                if (updatedEvent == null)
                {
                    return NotFound();
                }
                return Ok(updatedEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Event> Delete(int id)
        {
            var EventToDelete = _eventRepo.GetById(id);
            if (EventToDelete == null)
            {
                return NotFound($"Event with ID {id} not found.");
            }
            _eventRepo.Delete(id);
            return Ok(EventToDelete);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Event> GetById(int id)
        {
            var evt = _eventRepo.GetById(id);
            if (evt == null)
            {
                return NotFound($"Event with ID {id} not found.");
            }
            return Ok(evt);




        }
    }
}
