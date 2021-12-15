namespace AareonTechnicalTest.DTOs.Inbound.Tickets
{
    public class BaseInboundTicketDto
    {
        public BaseInboundTicketDto(string content, int personId)
        {
            Content = content;
            PersonId = personId;
        }

        public string Content { get; set; }
        public int PersonId { get; set; }
    }
}