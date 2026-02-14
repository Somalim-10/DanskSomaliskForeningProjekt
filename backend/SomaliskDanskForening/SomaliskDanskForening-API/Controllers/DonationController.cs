using Microsoft.AspNetCore.Mvc;
using SomaliskDanskForening_API.DTO_S;
using SomaliskDanskForening_Lib.Interfaces;
using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly IDonationRepo _donationRepo;

        public DonationController(IDonationRepo donationRepo)
        {
            _donationRepo = donationRepo;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var donations = _donationRepo.GetAll();
            if (donations == null || donations.Count == 0) return NotFound("No donations found.");
            return Ok(donations);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Donation> GetById(int id)
        {
            var donation = _donationRepo.GetById(id);
            if (donation == null) return NotFound($"Donation with ID {id} not found.");
            return Ok(donation);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Donation> Post([FromBody] DonationDTO donation)
        {
            try
            {
                var added = _donationRepo.Add(donation.ToDonation());
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
        public ActionResult<Donation> Put(int id, [FromBody] DonationDTO donation)
        {
            try
            {
                var d = donation.ToDonation();
                d.Id = id;
                var updated = _donationRepo.Update(d);
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
        public ActionResult<Donation> Delete(int id)
        {
            var existing = _donationRepo.GetById(id);
            if (existing == null) return NotFound($"Donation with ID {id} not found.");
            _donationRepo.Delete(id);
            return Ok(existing);
        }
    }
}
