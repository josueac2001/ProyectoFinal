using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SchoolPublications.DAL.Entities
{
    public class Comment: Entity
    {
        [Display(Name = "Comentarios")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Remark { get; set; } 
        public User User { get; set; }
        public Publication Publication { get; set; }
    }
}
