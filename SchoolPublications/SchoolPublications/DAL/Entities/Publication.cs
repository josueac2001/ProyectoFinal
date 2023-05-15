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
        public string Title { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7048/images/NoImage.png"
            : $"https://sales2023.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Fecha de publicacion")]
        public DateTime PublicationDate { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Category Category { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
