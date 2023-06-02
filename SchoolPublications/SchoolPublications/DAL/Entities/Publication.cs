
using System.ComponentModel.DataAnnotations;


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

        [Display(Name = "Fecha de publicacion")]
        public DateTime PublicationDate { get; set; }

        //[Display(Name = "Categoria")]
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //public ICollection<Comment> Comments { get; set; }
        public ICollection<PublicationCategory> PublicationCategories { get; set; }

        [Display(Name = "Categorías")]
        public int CategoriesNumber => PublicationCategories == null ? 0 : PublicationCategories.Count;

        public ICollection<PublicationImage> PublicationImages { get; set; }

        [Display(Name = "Número Fotos")]
        public int ImagesNumber => PublicationImages == null ? 0 : PublicationImages.Count;

        [Display(Name = "Foto")]
        public string ImageFullPath => PublicationImages == null || PublicationImages.Count == 0
            ? $"https://localhost:7048/images/NoImage.png"
            : PublicationImages.FirstOrDefault().ImageFullPath;

    }
}
