﻿using SchoolPublications.DAL;
using SchoolPublications.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SchoolPublications.Controllers
{
	[Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
	{
		private readonly DatabaseContext _context;

		public CategoriesController(DatabaseContext context)
		{
			_context = context;
		}

		// GET: Categories
		public async Task<IActionResult> Index()
		{
			return View(await _context.Categories.ToListAsync());
		}

		[Authorize(Roles = "Admin")]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Category category)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_context.Add(category);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (DbUpdateException dbUpdateException)
				{
					if (dbUpdateException.InnerException.Message.Contains("duplicate"))
					{
						ModelState.AddModelError(string.Empty, "Ya existe una categoría con el mismo nombre.");
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
			return View(category);
		}

		// GET: Categories/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
		{
			if (id == null || _context.Categories == null) return NotFound();

			Category category = await _context.Categories.FindAsync(id);
			if (category == null) return NotFound();
			return View(category);
		}

		// POST: Categories/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, Category category)
		{
			if (id != category.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(category);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (DbUpdateException dbUpdateException)
				{
					if (dbUpdateException.InnerException.Message.Contains("duplicate"))
					{
						ModelState.AddModelError(string.Empty, "Ya existe una categoría con el mismo nombre.");
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
			return View(category);
		}

		// GET: Categories/Details/5
		public async Task<IActionResult> Details(Guid? id)
		{
			if (id == null || _context.Categories == null)
			{
				return NotFound();
			}

			Category category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		// GET: Categories/Delete/5
		public async Task<IActionResult> Delete(Guid? id)
		{
			if (id == null || _context.Categories == null)
			{
				return NotFound();
			}

			Category category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		// POST: Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			if (_context.Categories == null)
			{
				return Problem("Entity set 'DatabaseContext.Categories'  is null.");
			}
			var category = await _context.Categories.FindAsync(id);
			if (category != null)
			{
				_context.Categories.Remove(category);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
