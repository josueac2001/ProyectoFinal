using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SchoolPublications.DAL.Entities
{
    public class Comment: Entity
    {
        [Display(Name = "Comentario")]
        [MaxLength(300, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Content { get; set; }

        [Display(Name = "Fecha de Comentario")]
        public DateTime CommentDate { get; set; }
        public User User { get; set; }

        [Display(Name = "Publicación")]
        public Publication Publication { get; set; }
    }
}
