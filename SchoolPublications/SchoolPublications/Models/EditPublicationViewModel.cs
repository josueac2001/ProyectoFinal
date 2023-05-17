
using SchoolPublications.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace SchoolPublications.Models
{
    public class EditPublicationViewModel : Entity
    {
        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Title { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [Display(Name = "Fecha de publicacion")]
        public DateTime PublicationDate { get; set; }
    }
}
