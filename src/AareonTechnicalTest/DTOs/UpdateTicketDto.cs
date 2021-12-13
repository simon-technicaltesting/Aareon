namespace AareonTechnicalTest.DTOs
{
    // Could use a base class as basically shared by create/update
    public class UpdateTicketDto
    {
        public UpdateTicketDto(string content, int personId)
        {
            Content = content;
            PersonId = personId;
        }

        public string Content { get; set; }
        public int PersonId { get; set; }
    }
}