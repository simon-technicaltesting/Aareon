namespace AareonTechnicalTest.DTOs.Inbound.Tickets
{
    public class UpdateTicketDto : BaseInboundTicketDto
    {
        public UpdateTicketDto(string content, int personId) : base(content, personId)
        {
        }
    }
}