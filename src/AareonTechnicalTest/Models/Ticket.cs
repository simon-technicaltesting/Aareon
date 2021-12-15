using AareonTechnicalTest.DTOs.Inbound.Tickets;
using System.Collections.Generic;

namespace AareonTechnicalTest.Models
{
    public class Ticket
    {
        public int Id { get; }
        public string Content { get; set; }
        public int PersonId { get; set; }
        public List<Note> Notes { get; init; }

        public void Update(UpdateTicketDto ticketUpdate)
        {
            Content = ticketUpdate.Content;
            PersonId = ticketUpdate.PersonId;
        }
    }
}