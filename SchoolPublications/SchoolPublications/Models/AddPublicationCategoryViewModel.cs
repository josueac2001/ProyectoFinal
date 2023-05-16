using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolPublications.Models
{
    public class AddPublicationCategoryViewModel
    {
        public Guid PublicationId { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } //Necesito la lista de las categorías para seleccionar una nueva categoría
    }
}
