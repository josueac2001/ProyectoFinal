using SchoolPublications.DAL.Entities;
namespace SchoolPublications.Models
{
    public class PublicationHomeViewModel
    {
        public ICollection<Publication> Publications { get; set; }
    }
}
