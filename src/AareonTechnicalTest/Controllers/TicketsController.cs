using AareonTechnicalTest.DTOs;
using AareonTechnicalTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AareonTechnicalTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ILogger<TicketsController> _logger;
        private readonly ApplicationContext _context;

        public TicketsController(
            ILogger<TicketsController> logger,
            ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(CreateTicketDto createTicket)
        {
            if (string.IsNullOrEmpty(createTicket.Content))
            {
                return BadRequest("Content is required");
            }

            var matchedPerson = await _context.Persons.FindAsync(createTicket.PersonId);
            if (matchedPerson == null)
            {
                return BadRequest("PersonId is invalid");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTicket = new Ticket {Content = createTicket.Content, PersonId = createTicket.PersonId};
            _context.Tickets.Add(newTicket);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Ticket '{Id}' created", newTicket.Id);

            return CreatedAtAction(nameof(GetTicket), new {id = newTicket.Id}, newTicket);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var matchedTicket = await _context.Tickets.FindAsync(id);
            if (matchedTicket != null)
            {
                return Ok(new TicketDto(matchedTicket));
            }

            _logger.LogInformation("Ticket '{Id}' requested, but not found", id);
            return NotFound();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTicket(int id, UpdateTicketDto ticketUpdate)
        {
            var matchedTicket = await _context.Tickets.FindAsync(id);
            if (matchedTicket == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(ticketUpdate.Content))
            {
                return BadRequest("Content is required");
            }

            var matchedPerson = await _context.Persons.FindAsync(ticketUpdate.PersonId);
            if (matchedPerson == null)
            {
                return BadRequest("PersonId is invalid");
            }

            try
            {
                matchedTicket.Update(ticketUpdate);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Ticket '{Id}' updated, content '{Content}', PersonId '{PersonId}'", id,
                    ticketUpdate.Content, ticketUpdate.PersonId);

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError("Ticket '{Id}' failed to update due to conflict", id);
                
                return Conflict("Data was updated during update request, please recheck and try again");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var matchedTicket = await _context.Tickets.FindAsync(id);
            if (matchedTicket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(matchedTicket);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Ticket '{Id}' deleted", id);

            return NoContent();
        }
    }
}