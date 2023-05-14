using SchoolPublications.DAL;
using SchoolPublications.Helpers;

namespace SchoolPublications.Services
{
    public class DropDownListsHelper : IDropDownListsHelper
    {
        private readonly DatabaseContext _context;

        public DropDownListsHelper(DatabaseContext context)
        {
            _context = context;
        }

    }
}
