using System;

namespace AareonTechnicalTest.Models
{
    public class Note
    {
        // Would normally make classes immutable, however more setup around ef core is required
        public int Id { get; }
        public string Content { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        
        public Ticket Ticket { get; set; }

        public void Update(string content, Ticket relatedTicket, int updatedById)
        {
            Content = content;
            Ticket = relatedTicket;
            LastUpdatedBy = updatedById;
            LastUpdatedAt = DateTime.Now;
        }
    }
}