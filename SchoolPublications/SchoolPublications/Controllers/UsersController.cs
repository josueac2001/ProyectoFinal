using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SchoolPublications.DAL;
using SchoolPublications.DAL.Entities;
using SchoolPublications.Enums;
using SchoolPublications.Helpers;
using SchoolPublications.Models;
using SchoolPublications.Services;
using System.Data;

namespace SchoolPublications.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController
    {
        private readonly IUserHelper _userHelper;
        private readonly DatabaseContext _context;
        //private readonly IDropDownListsHelper _ddlHelper;                         Necesarios para el DropDownLists
        //private readonly IAzureBlobHelper _azureBlobHelper;

        public UsersController(IUserHelper userHelper, DatabaseContext context, IDropDownListsHelper dropDownListsHelper, IAzureBlobHelper azureBlobHelper)
        {
            _userHelper = userHelper;
            _context = context;
            //_ddlHelper = dropDownListsHelper;                                             Igualmente esta parte, pero de momento quedará comentareada
            //_azureBlobHelper = azureBlobHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                .Include(u => u.Document)
                .ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> CreateAdmin()
        {
            Guid emptyGuid = new Guid();

            AddUserViewModel addUserViewModel = new()
            {
                Id = Guid.Empty,
                UserType = UserType.Admin,
            };

            return View(addUserViewModel);
        }

        private IActionResult View(object value)
        {
            throw new NotImplementedException();
        }
    }
}
