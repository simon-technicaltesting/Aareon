namespace AareonTechnicalTest.DTOs.Inbound.Tickets
{
    public class CreateTicketDto : BaseInboundTicketDto
    {
        public CreateTicketDto(string content, int personId) : base(content, personId)
        {
        }
    }
}