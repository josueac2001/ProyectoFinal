using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SchoolPublications.DAL.Entities
{
    public class Entity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

    }
}
