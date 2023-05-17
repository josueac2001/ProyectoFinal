using SchoolPublications.DAL;
using SchoolPublications.DAL.Entities;
using SchoolPublications.Helpers;
using SchoolPublications.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace SchoolPublications.Controllers
{
    [Authorize(Roles = "Admin")]
    [AllowAnonymous]
    public class PublicationsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IDropDownListsHelper _dropDownListsHelper;
        private readonly IAzureBlobHelper _azureBlobHelper;

        public PublicationsController(DatabaseContext context, IDropDownListsHelper dropDownListsHelper, IAzureBlobHelper azureBlobHelper)
        {
            _context = context;
            _dropDownListsHelper = dropDownListsHelper;
            _azureBlobHelper = azureBlobHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Publications
                .Include(p => p.PublicationImages)
                .Include(p => p.PublicationCategories)
                .ThenInclude(pc => pc.Category)
                .ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AddPublicationViewModel addPublicationViewModel = new()
            {
                Categories = await _dropDownListsHelper.GetDDLCategoriesAsync(),
            };

            return View(addPublicationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddPublicationViewModel addPublicationViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (addPublicationViewModel.ImageFile != null)
                        imageId = await _azureBlobHelper.UploadAzureBlobAsync(addPublicationViewModel.ImageFile, "publications");

                    Publication publication = new()
                    {
                    
                        Title = addPublicationViewModel.Title,
                        Description = addPublicationViewModel.Description,
                        PublicationDate = addPublicationViewModel.PublicationDate,
                    };

                    //Estoy capturando la categoría del publicacion para luego guardar esa relación en la tabla ProductCategories
                    publication.PublicationCategories = new List<PublicationCategory>()
                    {
                        new PublicationCategory
                        {
                            Category = await _context.Categories.FindAsync(addPublicationViewModel.CategoryId)
                        }
                    };

                    if (imageId != Guid.Empty)
                    {
                        publication.PublicationImages = new List<PublicationImage>()
                        {
                            new PublicationImage { ImageId = imageId }
                        };
                    }

                    _context.Add(publication);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una publicacion con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            addPublicationViewModel.Categories = await _dropDownListsHelper.GetDDLCategoriesAsync();
            return View(addPublicationViewModel);
        }

        public async Task<IActionResult> Edit(Guid? publicationId)
        {
            if (publicationId == null) return NotFound();

            Publication publication = await _context.Publications.FindAsync(publicationId);
            if (publication == null) return NotFound();

            EditPublicationViewModel editPublicationViewModel = new()
            {
                
                Id = publication.Id,
                Title = publication.Title,
                Description = publication.Description,
                PublicationDate = publication.PublicationDate,
            };

            return View(editPublicationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? Id, EditPublicationViewModel editPublicationViewModel)
        {
            if (Id != editPublicationViewModel.Id) return NotFound();

            try
            {
                Publication publication = await _context.Publications.FindAsync(editPublicationViewModel.Id);
                publication.Description = editPublicationViewModel.Description;
                publication.Title = editPublicationViewModel.Title;
                publication.PublicationDate = editPublicationViewModel.PublicationDate;
                _context.Update(publication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    ModelState.AddModelError(string.Empty, "Ya existe unA PUBLICACION con el mismo nombre.");
                else
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return View(editPublicationViewModel);
        }

        public async Task<IActionResult> Details(Guid? publicationId)
        {
            if (publicationId == null) return NotFound();

            Publication publication = await _context.Publications
                .Include(p => p.PublicationImages)
                .Include(p => p.PublicationCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == publicationId);
            if (publication == null) return NotFound();

            return View(publication);
        }

        public async Task<IActionResult> AddImage(Guid? publicationId)
        {
            if (publicationId == null) return NotFound();

            Publication publication = await _context.Publications.FindAsync(publicationId);
            if (publication == null) return NotFound();

            AddPublicationImageViewModel addPublicationImageViewModel = new()
            {
                PublicationId = publication.Id,
            };

            return View(addPublicationImageViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddPublicationImageViewModel addPublicationImageViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = await _azureBlobHelper.UploadAzureBlobAsync(addPublicationImageViewModel.ImageFile, "publications");

                    Publication publication = await _context.Publications.FindAsync(addPublicationImageViewModel.PublicationId);

                    PublicationImage publicationImage = new()
                    {
                        Publication = publication,
                        ImageId = imageId,
                    };

                    _context.Add(publicationImage);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { publicationId = publication.Id });
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(addPublicationImageViewModel);
        }

        public async Task<IActionResult> DeleteImage(Guid? imageId)
        {
            if (imageId == null) return NotFound();

            PublicationImage publicationImage = await _context.PublicationImages
                .Include(pi => pi.Publication)
                .FirstOrDefaultAsync(pi => pi.Id == imageId);
            
            if (publicationImage == null) return NotFound();

            await _azureBlobHelper.DeleteAzureBlobAsync(publicationImage.ImageId, "publications");
            _context.PublicationImages.Remove(publicationImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { publicationId = publicationImage.Publication.Id });
        }

        public async Task<IActionResult> AddCategory(Guid? publicationId)
        {
            if (publicationId == null) return NotFound();

            Publication publication = await _context.Publications
                .Include(p => p.PublicationCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == publicationId);

            if (publication == null) return NotFound();

            List<Category> categories = publication.PublicationCategories.Select(pc => new Category
            {
                Id = pc.Category.Id,
                Name = pc.Category.Name,
            }).ToList();

            AddPublicationCategoryViewModel addCategoryPublicationViewModel = new()
            {
                PublicationId = publication.Id,
                Categories = await _dropDownListsHelper.GetDDLCategoriesAsync(categories),
            };

            return View(addCategoryPublicationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(AddPublicationCategoryViewModel addCategoryPublicationViewModel)
        {
            //New change
            Publication publication = await _context.Publications
                .Include(p => p.PublicationCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == addCategoryPublicationViewModel.PublicationId);

            if (ModelState.IsValid)
            {
                try
                {
                    PublicationCategory publicationCategory = new()
                {
                    Category = await _context.Categories.FindAsync(addCategoryPublicationViewModel.CategoryId),
                    Publication = publication,
                };

                    _context.Add(publicationCategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { publicationId = publication.Id });
                }
                catch (Exception exception)
                {
                    addCategoryPublicationViewModel.Categories = await _dropDownListsHelper.GetDDLCategoriesAsync();
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            //New change
            List<Category> categories = publication.PublicationCategories.Select(pc => new Category
            {
                Id = pc.Category.Id,
                Name = pc.Category.Name,
            }).ToList();

            addCategoryPublicationViewModel.Categories = await _dropDownListsHelper.GetDDLCategoriesAsync(categories);
            return View(addCategoryPublicationViewModel);
        }


        public async Task<IActionResult> DeleteCategory(Guid? categoryId)
        {
            if (categoryId == null) return NotFound();

            PublicationCategory publicationCategory = await _context.PublicationCategories
                .Include(pc => pc.Publication)
                .FirstOrDefaultAsync(pc => pc.Id == categoryId);
            if (publicationCategory == null) return NotFound();

            _context.PublicationCategories.Remove(publicationCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { publicationId = publicationCategory.Publication.Id });
        }

        public async Task<IActionResult> Delete(Guid? publicationId)
        {
            if (publicationId == null) return NotFound();

            Publication publication = await _context.Publications
                .Include(p => p.PublicationCategories)
                .Include(p => p.PublicationImages)
                .FirstOrDefaultAsync(p => p.Id == publicationId);
            
            if (publication == null) return NotFound();

            return View(publication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Publication publicationModel)
        {
            Publication publication = await _context.Publications
                .Include(p => p.PublicationImages)
                .Include(p => p.PublicationCategories)
                .FirstOrDefaultAsync(p => p.Id == publicationModel.Id);

            _context.Publications.Remove(publication);
            await _context.SaveChangesAsync();

            foreach (PublicationImage publicationImage in publication.PublicationImages)
                await _azureBlobHelper.DeleteAzureBlobAsync(publicationImage.ImageId, "publication");

            return RedirectToAction(nameof(Index));
        }
    }
}
