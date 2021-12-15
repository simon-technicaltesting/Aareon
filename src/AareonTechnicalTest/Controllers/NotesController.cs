using AareonTechnicalTest.DataAccess;
using AareonTechnicalTest.DTOs;
using AareonTechnicalTest.DTOs.Inbound.Notes;
using AareonTechnicalTest.Exceptions;
using AareonTechnicalTest.Extensions;
using AareonTechnicalTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AareonTechnicalTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly ILogger<NotesController> _logger;
        private readonly INoteDataAccess _noteDataAccess;
        private readonly ITicketDataAccess _ticketDataAccess;
        private readonly IPersonDataAccess _personDataAccess;

        public NotesController(
            ILogger<NotesController> logger,
            INoteDataAccess noteDataAccess,
            ITicketDataAccess ticketDataAccess,
            IPersonDataAccess personDataAccess)
        {
            _logger = logger;
            _noteDataAccess = noteDataAccess;
            _ticketDataAccess = ticketDataAccess;
            _personDataAccess = personDataAccess;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote(CreateNoteDto createNote)
        {
            // Getting person id from header rather than submission as in more real world its likely to be coming from a bearer token etc
            var createdByAsString = Request.GetHeader("CreatedBy");
            if (string.IsNullOrEmpty(createdByAsString))
            {
                return BadRequest("CreatedBy is required");
            }

            // Would move to something like fluent validations to tidy logic
            if (int.TryParse(createdByAsString, out var createdById))
            {
                return BadRequest("Valid CreatedBy Id is required");
            }

            if (string.IsNullOrEmpty(createNote.Content))
            {
                return BadRequest("Content is required");
            }

            var createdBy = await _personDataAccess.GetPerson(createdById);
            if (createdBy == null)
            {
                return BadRequest("Created By id is invalid or not found");
            }

            var newNote = new Note { Content = createNote.Content, CreatedBy = createdById };

            if (createNote.TicketId.HasValue)
            {
                var matchedTicket = await _ticketDataAccess.GetTicket(createNote.TicketId.Value);
                if (matchedTicket == null)
                {
                    return BadRequest("Ticket id is invalid or not found");
                }

                newNote.Ticket = matchedTicket;
            }
             
            var addedNote = await _noteDataAccess.AddNote(newNote);

            _logger.LogInformation("Note '{Id}' created", addedNote.Id);

            return CreatedAtAction(nameof(GetNote), new { id = addedNote.Id }, new NoteDto(addedNote));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetNote(int id)
        {
            var matchedNote = await _noteDataAccess.GetNote(id);
            if (matchedNote != null)
            {
                return Ok(new NoteDto(matchedNote));
            }

            _logger.LogInformation("Note '{Id}' requested, but not found", id);
            return NotFound();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTicket(int id, UpdateNoteDto noteUpdate)
        {
            // Getting person id from header rather than submission as in more real world its likely to be coming from a bearer token etc
            var updatedByAsString = Request.GetHeader("UpdatedBy");
            if (string.IsNullOrEmpty(updatedByAsString))
            {
                return BadRequest("UpdatedBy is required");
            }
            
            // Would move to something like fluent validations to tidy logic
            if (int.TryParse(updatedByAsString, out var updatedById))
            {
                return BadRequest("Valid UpdatedBy Id is required");
            }
            
            var updatedBy = await _personDataAccess.GetPerson(updatedById);
            if (updatedBy == null)
            {
                return BadRequest("Created By id is invalid or not found");
            }

            var noteToUpdate = await _noteDataAccess.GetNote(id);
            if (noteToUpdate == null)
            {
                return NotFound();
            }
            
            if (string.IsNullOrEmpty(noteUpdate.Content))
            {
                return BadRequest("Note content is required");
            }

            Ticket relatedTicket = null;
            if (noteUpdate.TicketId.HasValue)
            {
                relatedTicket = await _ticketDataAccess.GetTicket(noteUpdate.TicketId.Value);
                if (relatedTicket == null)
                {
                    return BadRequest("Created By id is invalid or not found");
                }
            }
            
            try
            {
                var updatedNote = await _noteDataAccess.UpdateNote(noteToUpdate, noteUpdate.Content, relatedTicket, updatedById);
        
                _logger.LogInformation("Note '{Id}' updated, content '{Content}', TicketId '{PersonId}'", id,
                    updatedNote.Content, relatedTicket == null ? "n/a" : relatedTicket.Id);
        
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError("Note '{Id}' failed to update due to conflict", id);
        
                return Conflict("Data was updated during update request, please recheck and try again");
            }
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            // Getting person id from header rather than submission as in more real world its likely to be coming from a bearer token etc
            var deletedByAsString = Request.GetHeader("DeletedBy");
            if (string.IsNullOrEmpty(deletedByAsString))
            {
                return BadRequest("DeletedBy is required");
            }
            
            if (int.TryParse(deletedByAsString, out var deletedBy))
            {
                return BadRequest("Valid DeletedBy Id is required");
            }
            
            var matchedPerson = await _personDataAccess.GetPerson(deletedBy);
            if (matchedPerson == null)
            {
                return BadRequest("DeletedBy id is invalid or not found");
            }

            if (!matchedPerson.IsAdmin)
            {
                _logger.LogWarning("User '{DeletedBy}' attempted to delete Note '{Id}'", deletedBy, id);
                return Unauthorized();
            }
            
            try
            {
                await _noteDataAccess.DeleteNote(id);
                _logger.LogInformation("Note '{Id}' deleted", id);
                return NoContent();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }
    }
}