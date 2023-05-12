using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SchoolPublications.DAL.Entities
{
    public class Approval : Entity
    {
   
        [Display(Name = "Fecha de aprobacion")]
        public DateTime ApprovalDate { get; set; }

        public User User { get; set; }
        public Publication Publication { get; set; }

    }
}
