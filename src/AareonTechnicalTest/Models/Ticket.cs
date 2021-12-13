using AareonTechnicalTest.DTOs;
using System.ComponentModel.DataAnnotations;

namespace AareonTechnicalTest.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; }
        
        public string Content { get; set; }
        public int PersonId { get; set; }

        public void Update(UpdateTicketDto ticketUpdate)
        {
            Content = ticketUpdate.Content;
            PersonId = ticketUpdate.PersonId;
        }
    }
}
