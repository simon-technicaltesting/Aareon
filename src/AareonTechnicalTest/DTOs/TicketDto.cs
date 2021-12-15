using AareonTechnicalTest.Models;
using System.Collections.Generic;
using System.Linq;

namespace AareonTechnicalTest.DTOs
{
    public class TicketDto
    {
        public TicketDto(Ticket ticket)
        {
            Id = ticket.Id;
            Content = ticket.Content;
            PersonId = ticket.PersonId;
            Notes = ticket.Notes.Select(x => new NoteDto(x)).ToList();
        }

        public int Id { get; }
        public string Content { get; }
        public int PersonId { get; }

        public List<NoteDto> Notes { get; }
    }
}