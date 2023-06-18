using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FileToEmailLinker.Data;
using FileToEmailLinker.Models;

namespace FileToEmailLinker.Controllers
{
    public class ReceiversController : Controller
    {
        private readonly FileToEmailLinkerContext _context;

        public ReceiversController(FileToEmailLinkerContext context)
        {
            _context = context;
        }

        // GET: Receivers
        public async Task<IActionResult> Index()
        {
              return _context.Receiver != null ? 
                          View(await _context.Receiver.ToListAsync()) :
                          Problem("Entity set 'FileToEmailLinkerContext.Receiver'  is null.");
        }

        // GET: Receivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Receiver == null)
            {
                return NotFound();
            }

            var receiver = await _context.Receiver
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receiver == null)
            {
                return NotFound();
            }

            return View(receiver);
        }

        // GET: Receivers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Receivers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Email")] Receiver receiver)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receiver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(receiver);
        }

        // GET: Receivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Receiver == null)
            {
                return NotFound();
            }

            var receiver = await _context.Receiver.FindAsync(id);
            if (receiver == null)
            {
                return NotFound();
            }
            return View(receiver);
        }

        // POST: Receivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Email")] Receiver receiver)
        {
            if (id != receiver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receiver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiverExists(receiver.Id))
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
            return View(receiver);
        }

        // GET: Receivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Receiver == null)
            {
                return NotFound();
            }

            var receiver = await _context.Receiver
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receiver == null)
            {
                return NotFound();
            }

            return View(receiver);
        }

        // POST: Receivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Receiver == null)
            {
                return Problem("Entity set 'FileToEmailLinkerContext.Receiver'  is null.");
            }
            var receiver = await _context.Receiver.FindAsync(id);
            if (receiver != null)
            {
                _context.Receiver.Remove(receiver);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiverExists(int id)
        {
          return (_context.Receiver?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
