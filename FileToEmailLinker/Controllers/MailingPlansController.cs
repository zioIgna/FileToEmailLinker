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
    public class MailingPlansController : Controller
    {
        private readonly FileToEmailLinkerContext _context;

        public MailingPlansController(FileToEmailLinkerContext context)
        {
            _context = context;
        }

        // GET: MailingPlans
        public async Task<IActionResult> Index()
        {
            var fileToEmailLinkerContext = _context.MailingPlan.Include(m => m.Schedulation);
            return View(await fileToEmailLinkerContext.ToListAsync());
        }

        // GET: MailingPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MailingPlan == null)
            {
                return NotFound();
            }

            var mailingPlan = await _context.MailingPlan
                .Include(m => m.Schedulation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mailingPlan == null)
            {
                return NotFound();
            }

            return View(mailingPlan);
        }

        // GET: MailingPlans/Create
        public IActionResult Create()
        {
            ViewData["SchedulationId"] = new SelectList(_context.Set<Schedulation>(), "Id", "Name");
            return View();
        }

        // POST: MailingPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ActiveState,Text,SchedulationId")] MailingPlan mailingPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mailingPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchedulationId"] = new SelectList(_context.Set<Schedulation>(), "Id", "Id", mailingPlan.SchedulationId);
            return View(mailingPlan);
        }

        // GET: MailingPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MailingPlan == null)
            {
                return NotFound();
            }

            var mailingPlan = await _context.MailingPlan.FindAsync(id);
            if (mailingPlan == null)
            {
                return NotFound();
            }
            ViewData["SchedulationId"] = new SelectList(_context.Set<Schedulation>(), "Id", "Name", mailingPlan.SchedulationId);
            return View(mailingPlan);
        }

        // POST: MailingPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ActiveState,Text,SchedulationId")] MailingPlan mailingPlan)
        {
            if (id != mailingPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mailingPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MailingPlanExists(mailingPlan.Id))
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
            ViewData["SchedulationId"] = new SelectList(_context.Set<Schedulation>(), "Id", "Id", mailingPlan.SchedulationId);
            return View(mailingPlan);
        }

        // GET: MailingPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MailingPlan == null)
            {
                return NotFound();
            }

            var mailingPlan = await _context.MailingPlan
                .Include(m => m.Schedulation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mailingPlan == null)
            {
                return NotFound();
            }

            return View(mailingPlan);
        }

        // POST: MailingPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MailingPlan == null)
            {
                return Problem("Entity set 'FileToEmailLinkerContext.MailingPlan'  is null.");
            }
            var mailingPlan = await _context.MailingPlan.FindAsync(id);
            if (mailingPlan != null)
            {
                _context.MailingPlan.Remove(mailingPlan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MailingPlanExists(int id)
        {
          return (_context.MailingPlan?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
