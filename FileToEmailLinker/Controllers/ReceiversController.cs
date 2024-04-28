using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FileToEmailLinker.Data;
using FileToEmailLinker.Models.Entities;
using FileToEmailLinker.Models.InputModels.Receivers;
using FileToEmailLinker.Models.Services.Receiver;
using FileToEmailLinker.Models.Exceptions;
using NuGet.Protocol.Plugins;
using FileToEmailLinker.Models.ViewModels;

namespace FileToEmailLinker.Controllers
{
    public class ReceiversController : Controller
    {
        private readonly FileToEmailLinkerContext _context;
        private readonly IReceiverService receiverService;

        public ReceiversController(FileToEmailLinkerContext context, IReceiverService receiverService)
        {
            _context = context;
            this.receiverService = receiverService;
        }

        // GET: Receivers
        public async Task<IActionResult> Index(int page = 1, int limit = 10, string search = "")
        {
            ReceiverListViewModel model = await receiverService.GetReceiverListViewModelAsync(page, limit, search);
            return View(model);
        }

        // GET: Receivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Models.Entities.Receiver receiver = await receiverService.GetReceiverByIdAsync((int)id);
            if(receiver == null)
            {
                TempData["ErrorMessage"] = "Non è stato possibile recuperare il destinatario cercato";
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Email")] Models.Entities.Receiver model)
        {
            if (ModelState.IsValid)
            {
                Models.Entities.Receiver receiver = await receiverService.CreateReceiverAsync(model);
                TempData["ConfirmationMessage"] = "Destinatario creato con successo";
                return RedirectToAction(nameof(Details), new { id = receiver.Id });
            }
            return View(model);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Email")] Models.Entities.Receiver receiver)
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

        public async Task<IActionResult> EditInline(Models.Entities.Receiver receiver)
        {
            if (receiver == null || receiver.Id == 0)
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
                return PartialView("/Views/Shared/Output/_ReceiverInlineOutput.cshtml", receiver);
            }
            Models.Entities.Receiver originalReceiver = await receiverService.GetReceiverByIdAsync(receiver.Id);
            return PartialView("/Views/Shared/Output/_ReceiverInlineOutput.cshtml", originalReceiver);
        }

        public async Task<IActionResult> InlineOutput(int id)
        {
            Models.Entities.Receiver receiver = await receiverService.GetReceiverByIdAsync(id);
            if (receiver == null)
            {
                return NotFound();
            }
            else
            {
                return PartialView("/Views/Shared/Output/_ReceiverInlineOutput.cshtml", receiver);
            }
        }

        public async Task<IActionResult> InlineInput(int id)
        {
            Models.Entities.Receiver receiver = await receiverService.GetReceiverByIdAsync(id);
            if (receiver == null)
            {
                return NotFound();
            }
            else
            {
                return PartialView("/Views/Shared/Input/_ReceiverInlineInput.cshtml", receiver);
            }
        }

        // GET: Receivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Models.Entities.Receiver receiver = await receiverService.GetReceiverByIdAsync((int)id);
            if (receiver == null)
            {
                TempData["ErrorMessage"] = "Non è stato possibile recuperare il destinatario cercato";
                return RedirectToAction(nameof(Index));
            }
            bool canDeleteReceiver = !receiver.MailingPlanList.Any();
            if (canDeleteReceiver)
            {
                return View(receiver);
            }
            TempData["ErrorMessage"] = "Non è possibile eliminare il destinatario in quanto collegato a una o più pianificazioni. Eliminare prima il link a tali pianificazioni";
            return RedirectToAction(nameof(Index));
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
