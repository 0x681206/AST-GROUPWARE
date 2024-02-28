using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_groupware.Data;
using web_groupware.Models;

namespace web_groupware.Controllers
{
    public class T_STAFFMController : Controller
    {
        private readonly web_groupwareContext _context;

        public T_STAFFMController(web_groupwareContext context)
        {
            _context = context;
        }

        // GET: T_STAFFM
        public async Task<IActionResult> Index()
        {
              return _context.T_STAFFM != null ? 
                          View(await _context.T_STAFFM.ToListAsync()) :
                          Problem("Entity set 'web_groupwareContext.T_STAFFM'  is null.");
        }

        // GET: T_STAFFM/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.T_STAFFM == null)
            {
                return NotFound();
            }

            var t_STAFFM = await _context.T_STAFFM
                .FirstOrDefaultAsync(m => m.staf_cd == id);
            if (t_STAFFM == null)
            {
                return NotFound();
            }

            return View(t_STAFFM);
        }

        // GET: T_STAFFM/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: T_STAFFM/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("staf_cd,password,staf_name,auth_admin,mail")] T_STAFFM t_STAFFM)
        {
            if (ModelState.IsValid)
            {
                _context.Add(t_STAFFM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(t_STAFFM);
        }

        // GET: T_STAFFM/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.T_STAFFM == null)
            {
                return NotFound();
            }

            var t_STAFFM = await _context.T_STAFFM.FindAsync(id);
            if (t_STAFFM == null)
            {
                return NotFound();
            }
            return View(t_STAFFM);
        }

        // POST: T_STAFFM/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("staf_cd,password,staf_name,auth_admin,mail")] T_STAFFM t_STAFFM)
        {
            if (id != t_STAFFM.staf_cd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(t_STAFFM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!T_STAFFMExists(t_STAFFM.staf_cd))
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
            return View(t_STAFFM);
        }

        // GET: T_STAFFM/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.T_STAFFM == null)
            {
                return NotFound();
            }

            var t_STAFFM = await _context.T_STAFFM
                .FirstOrDefaultAsync(m => m.staf_cd == id);
            if (t_STAFFM == null)
            {
                return NotFound();
            }

            return View(t_STAFFM);
        }

        // POST: T_STAFFM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.T_STAFFM == null)
            {
                return Problem("Entity set 'web_groupwareContext.T_STAFFM'  is null.");
            }
            var t_STAFFM = await _context.T_STAFFM.FindAsync(id);
            if (t_STAFFM != null)
            {
                _context.T_STAFFM.Remove(t_STAFFM);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool T_STAFFMExists(int id)
        {
          return (_context.T_STAFFM?.Any(e => e.staf_cd == id)).GetValueOrDefault();
        }
    }
}
