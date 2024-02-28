using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_groupware.Data;

namespace web_groupware.Models
{
    public class ScheduleViewModelsController : Controller
    {
        private readonly web_groupwareContext _context;

        public ScheduleViewModelsController(web_groupwareContext context)
        {
            _context = context;
        }

        // GET: ScheduleViewModels
        public async Task<IActionResult> Index()
        {
              return _context.ScheduleViewModel != null ? 
                          View(await _context.ScheduleViewModel.ToListAsync()) :
                          Problem("Entity set 'web_groupwareContext.ScheduleViewModel'  is null.");
        }

        // GET: ScheduleViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ScheduleViewModel == null)
            {
                return NotFound();
            }

            var scheduleViewModel = await _context.ScheduleViewModel
                .FirstOrDefaultAsync(m => m.schedule_no == id);
            if (scheduleViewModel == null)
            {
                return NotFound();
            }

            return View(scheduleViewModel);
        }

        // GET: ScheduleViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScheduleViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("schedule_no,schedule_type,allday,start_datetime,end_datetime,title,memo")] ScheduleViewModel scheduleViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scheduleViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scheduleViewModel);
        }

        // GET: ScheduleViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ScheduleViewModel == null)
            {
                return NotFound();
            }

            var scheduleViewModel = await _context.ScheduleViewModel.FindAsync(id);
            if (scheduleViewModel == null)
            {
                return NotFound();
            }
            return View(scheduleViewModel);
        }

        // POST: ScheduleViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("schedule_no,schedule_type,allday,start_datetime,end_datetime,title,memo")] ScheduleViewModel scheduleViewModel)
        {
            if (id != scheduleViewModel.schedule_no)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scheduleViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleViewModelExists(scheduleViewModel.schedule_no))
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
            return View(scheduleViewModel);
        }

        // GET: ScheduleViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ScheduleViewModel == null)
            {
                return NotFound();
            }

            var scheduleViewModel = await _context.ScheduleViewModel
                .FirstOrDefaultAsync(m => m.schedule_no == id);
            if (scheduleViewModel == null)
            {
                return NotFound();
            }

            return View(scheduleViewModel);
        }

        // POST: ScheduleViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ScheduleViewModel == null)
            {
                return Problem("Entity set 'web_groupwareContext.ScheduleViewModel'  is null.");
            }
            var scheduleViewModel = await _context.ScheduleViewModel.FindAsync(id);
            if (scheduleViewModel != null)
            {
                _context.ScheduleViewModel.Remove(scheduleViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleViewModelExists(int id)
        {
          return (_context.ScheduleViewModel?.Any(e => e.schedule_no == id)).GetValueOrDefault();
        }
    }
}
