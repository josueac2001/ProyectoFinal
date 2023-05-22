namespace SchoolPublications.DAL.Entities
{
    public class PublicationCategory : Entity
    {
        public Publication Publication { get; set; }
        public Category Category { get; set; }
    }
}
