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
    public class SchedulationsController : Controller
    {
        private readonly FileToEmailLinkerContext _context;

        public SchedulationsController(FileToEmailLinkerContext context)
        {
            _context = context;
        }

        // GET: Schedulations
        public async Task<IActionResult> Index()
        {
              return _context.Schedulation != null ? 
                          View(await _context.Schedulation.ToListAsync()) :
                          Problem("Entity set 'FileToEmailLinkerContext.Schedulation'  is null.");
        }

        // GET: Schedulations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Schedulation == null)
            {
                return NotFound();
            }

            var schedulation = await _context.Schedulation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedulation == null)
            {
                return NotFound();
            }

            return View(schedulation);
        }

        // GET: Schedulations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Schedulations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Recurrence,Date,Time")] Schedulation schedulation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedulation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(schedulation);
        }

        // GET: Schedulations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Schedulation == null)
            {
                return NotFound();
            }

            var schedulation = await _context.Schedulation.FindAsync(id);
            if (schedulation == null)
            {
                return NotFound();
            }
            return View(schedulation);
        }

        // POST: Schedulations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Recurrence,Date,Time")] Schedulation schedulation)
        {
            if (id != schedulation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedulation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchedulationExists(schedulation.Id))
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
            return View(schedulation);
        }

        // GET: Schedulations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Schedulation == null)
            {
                return NotFound();
            }

            var schedulation = await _context.Schedulation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedulation == null)
            {
                return NotFound();
            }

            return View(schedulation);
        }

        // POST: Schedulations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Schedulation == null)
            {
                return Problem("Entity set 'FileToEmailLinkerContext.Schedulation'  is null.");
            }
            var schedulation = await _context.Schedulation.FindAsync(id);
            if (schedulation != null)
            {
                _context.Schedulation.Remove(schedulation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchedulationExists(int id)
        {
          return (_context.Schedulation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
