using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SchoolPublications.DAL.Entities
{
    public class Publication :Entity
    {
        [Display(Name = "Titulo")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Tittle { get; set; }

        [Display(Name = "Contenido")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        public string Content { get; set; }

     
        [Display(Name = "Fecha de publicacion")]
        public DateTime PublicationDate { get; set; }

        public Boolean State { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Category Category { get; set; }
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
