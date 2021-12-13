using AareonTechnicalTest.Models;

namespace AareonTechnicalTest.DTOs
{
    public class TicketDto
    {
        public TicketDto(Ticket ticket)
        {
            Id = ticket.Id;
            Content = ticket.Content;
            PersonId = ticket.PersonId;
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public int PersonId { get; set; }
    }
}