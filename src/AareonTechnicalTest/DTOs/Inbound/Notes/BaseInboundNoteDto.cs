namespace AareonTechnicalTest.DTOs.Inbound.Notes
{
    public class BaseInboundNoteDto
    {
        public BaseInboundNoteDto(string content, int? ticketId)
        {
            Content = content;
            TicketId = ticketId;
        }

        public string Content { get; set; }
        public int? TicketId { get; set; }
    }
}