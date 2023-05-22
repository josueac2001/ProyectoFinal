using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolPublications.DAL.Entities;

namespace SchoolPublications.Helpers
{
    public interface IDropDownListsHelper 
    {
        Task<IEnumerable<SelectListItem>> GetDDLCategoriesAsync();
        Task<IEnumerable<SelectListItem>> GetDDLCategoriesAsync(IEnumerable<Category> filterCategories);

    }
}
