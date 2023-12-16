using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FileToEmailLinker.Data;
using FileToEmailLinker.Models.Entities;

namespace FileToEmailLinker.Controllers
{
    public class FileRefsController : Controller
    {
        private readonly FileToEmailLinkerContext _context;

        public FileRefsController(FileToEmailLinkerContext context)
        {
            _context = context;
        }

        // GET: FileRefs
        public async Task<IActionResult> Index()
        {
              return _context.FileRef != null ? 
                          View(await _context.FileRef.ToListAsync()) :
                          Problem("Entity set 'FileToEmailLinkerContext.FileRef'  is null.");
        }

        // GET: FileRefs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FileRef == null)
            {
                return NotFound();
            }

            var fileRef = await _context.FileRef
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileRef == null)
            {
                return NotFound();
            }

            return View(fileRef);
        }

        // GET: FileRefs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FileRefs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] FileRef fileRef)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fileRef);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fileRef);
        }

        // GET: FileRefs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FileRef == null)
            {
                return NotFound();
            }

            var fileRef = await _context.FileRef.FindAsync(id);
            if (fileRef == null)
            {
                return NotFound();
            }
            return View(fileRef);
        }

        // POST: FileRefs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] FileRef fileRef)
        {
            if (id != fileRef.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fileRef);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileRefExists(fileRef.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fileRef);
        }

        // GET: FileRefs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FileRef == null)
            {
                return NotFound();
            }

            var fileRef = await _context.FileRef
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileRef == null)
            {
                return NotFound();
            }

            return View(fileRef);
        }

        // POST: FileRefs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FileRef == null)
            {
                return Problem("Entity set 'FileToEmailLinkerContext.FileRef'  is null.");
            }
            var fileRef = await _context.FileRef.FindAsync(id);
            if (fileRef != null)
            {
                _context.FileRef.Remove(fileRef);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileRefExists(int id)
        {
          return (_context.FileRef?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
