using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FileToEmailLinker.Data;
using FileToEmailLinker.Models.Entities;
using FileToEmailLinker.Models.Services.MailingPlan;
using FileToEmailLinker.Models.InputModels.MailPlans;
using FileToEmailLinker.Models.InputModels.Schedulations;
using Org.BouncyCastle.Bcpg;

namespace FileToEmailLinker.Controllers
{
    public class MailingPlansController : Controller
    {
        private readonly FileToEmailLinkerContext _context;
        private readonly IMailingPlanService mailingPlanService;

        public MailingPlansController(FileToEmailLinkerContext context, IMailingPlanService mailingPlanService)
        {
            _context = context;
            this.mailingPlanService = mailingPlanService;
        }

        // GET: MailingPlans
        public async Task<IActionResult> Index()
        {
            //var fileToEmailLinkerContext = _context.MailingPlan.Include(m => m.Schedulation);
            //return View(await fileToEmailLinkerContext.ToListAsync());
            ICollection<MailingPlan> mailinPlanList = await mailingPlanService.GetMailingPlanListAsync();
            return View(mailinPlanList);
        }

        // GET: MailingPlans/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var mailingPlan = await mailingPlanService.GetMailingPlanByIdAsync(id);
            if(mailingPlan == null)
            {
                TempData["ErrorMessage"] = "Non è stato possibile recuperare la pianificazione cercata";
                return RedirectToAction(nameof(Index));
            }

            return View(mailingPlan);
        }

        // GET: MailingPlans/Create
        public async Task<IActionResult> Create()
        {
            //ViewData["SchedulationId"] = new SelectList(_context.Set<Schedulation>(), "Id", "Name");
            MailPlanInputModel mailingPlanInputModel = await mailingPlanService.CreateMailPlanInputModelAsync();
            return View(mailingPlanInputModel);
        }

        // POST: MailingPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MailPlanInputModel model) //MailPlanCreateInputModel model
        {
            ValidateSchedules(model);
            if (ModelState.IsValid)
            {
                MailingPlan mailingPlan = await mailingPlanService.CreateMailingPlanAsync(model);
                TempData["ConfirmationMessage"] = "Pianificazione creata con successo";
                return RedirectToAction(nameof(Details), new { id = mailingPlan.Id });
            }

            string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

            MailPlanInputModel restoredModel = await mailingPlanService.RestoreModelForCreation(model);
            return View(restoredModel);
        }

        private void ValidateSchedules(MailPlanInputModel model)
        {
            if (model.WeeklySchedulation == null && model.MonthlySchedulation == null)
            {
                ModelState.AddModelError(nameof(model.WeeklySchedulation), "Non sono stati selezionati giorni per l'invio");
            }
            else if (model.MonthlySchedulation != null && !DayAndMonthSelected(model.MonthlySchedulation))
            {
                ModelState.AddModelError(nameof(model.MonthlySchedulation), "Selezionare almeno un giorno e un mese per la schedulazione mensile");
            }
        }

        private bool DayAndMonthSelected(MonthlyScheduleInputModel monthlySchedulation)
        {
            return (
                (monthlySchedulation.One ||
                monthlySchedulation.Two ||
                monthlySchedulation.Three ||
                monthlySchedulation.Four ||
                monthlySchedulation.Five ||
                monthlySchedulation.Six ||
                monthlySchedulation.Seven ||
                monthlySchedulation.Eight ||
                monthlySchedulation.Nine ||
                monthlySchedulation.Ten ||
                monthlySchedulation.Eleven ||
                monthlySchedulation.Twelve ||
                monthlySchedulation.Thirteen ||
                monthlySchedulation.Fourteen ||
                monthlySchedulation.Fifteen ||
                monthlySchedulation.Sixteen ||
                monthlySchedulation.Seventeen ||
                monthlySchedulation.Eighteen ||
                monthlySchedulation.Nineteen ||
                monthlySchedulation.Twenty ||
                monthlySchedulation.Twentyone ||
                monthlySchedulation.Twentytwo ||
                monthlySchedulation.Twentythree ||
                monthlySchedulation.Twentyfour ||
                monthlySchedulation.Twentyfive ||
                monthlySchedulation.Twentysix ||
                monthlySchedulation.Twentyseven ||
                monthlySchedulation.Twentyeight ||
                monthlySchedulation.Twentynine ||
                monthlySchedulation.Thirty ||
                monthlySchedulation.Thirtyone) &&
                (monthlySchedulation.January||
                monthlySchedulation.February||
                monthlySchedulation.March||
                monthlySchedulation.April||
                monthlySchedulation.May||
                monthlySchedulation.June||
                monthlySchedulation.July||
                monthlySchedulation.August||
                monthlySchedulation.September||
                monthlySchedulation.October||
                monthlySchedulation.November||
                monthlySchedulation.December));
        }

        public IActionResult AddWeeklyScheduleInputModel()
        {
            WeeklyScheduleInputModel model = new();
            return PartialView("/Views/Schedulations/_WeeklySchedulation.cshtml", model);
        }

        public IActionResult AddMonthlyScheduleInputModel()
        {
            MonthlyScheduleInputModel model = new();
            return PartialView("/Views/Schedulations/_MonthlySchedulation.cshtml", model);
        }

        // GET: MailingPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id is null)
            {
                ViewData["ErrorMessage"] = "Errato riferimento per la programmazione";
                return View(nameof(Index));
            }
            MailingPlan mailingPlan = await mailingPlanService.GetMailingPlanByIdAsync((int)id);
            if(mailingPlan is null)
            {
                ViewData["ErrorMessage"] = "Non è possibile recuperare la programmazione cercata";
                return View(nameof(Index));
            }
            MailPlanInputModel mailPlanInputModel = await mailingPlanService.GetMailingPlanEditModelAsync((int)id);

            return View(mailPlanInputModel);
        }

        // POST: MailingPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MailPlanInputModel model)
        {
            ValidateSchedules(model);
            if (ModelState.IsValid)
            {
                MailingPlan mailingPlan = await mailingPlanService.EditMailingPlanAsync(model);
                TempData["ConfirmationMessage"] = "Pianificazione modificata con successo";
                return RedirectToAction(nameof(Details), new { id = mailingPlan.Id });
            }

            string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

            MailPlanInputModel restoredModel = await mailingPlanService.RestoreModelForCreation(model);
            return View(restoredModel);
        }

        // GET: MailingPlans/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var mailingPlan = await mailingPlanService.GetMailingPlanByIdAsync(id);
            if (mailingPlan == null)
            {
                TempData["ErrorMessage"] = "Non è stato possibile recuperare la pianificazione cercata";
                return RedirectToAction(nameof(Index));
            }

            return View(mailingPlan);
        }

        // POST: MailingPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mailingPlan = await mailingPlanService.GetMailingPlanByIdAsync(id);
            if (mailingPlan == null)
            {
                TempData["ErrorMessage"] = "Non è stato possibile recuperare la pianificazione cercata";
            }
            else
            {
                await mailingPlanService.DeleteMailingPlanAsync(mailingPlan);
                TempData["ConfirmationMessage"] = "Pianificazione eliminata correttamente";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MailingPlanExists(int id)
        {
          return (_context.MailingPlan?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
