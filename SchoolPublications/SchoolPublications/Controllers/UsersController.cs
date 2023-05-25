using SchoolPublications.DAL;
using SchoolPublications.DAL.Entities;
using SchoolPublications.Enums;
using SchoolPublications.Helpers;
using SchoolPublications.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SchoolPublications.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DatabaseContext _context;
        private readonly IAzureBlobHelper _azureBlobHelper;

        public UsersController(IUserHelper userHelper, DatabaseContext context, IAzureBlobHelper azureBlobHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _azureBlobHelper = azureBlobHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(AddUserViewModel addUserViewModel)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (addUserViewModel.ImageFile != null)
                    imageId = await _azureBlobHelper.UploadAzureBlobAsync(addUserViewModel.ImageFile, "users");

                addUserViewModel.ImageId = imageId;

                User user = await _userHelper.AddUserAsync(addUserViewModel);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    return View(addUserViewModel);
                }

                return RedirectToAction("Index", "Users");
            }

            return View(addUserViewModel);
        }

    }
}
