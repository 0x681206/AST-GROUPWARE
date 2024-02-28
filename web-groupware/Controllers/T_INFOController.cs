//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using web_groupware.Data;
//using web_groupware.Models;

//namespace web_groupware.Controllers
//{
//    public class T_INFOController : Controller
//    {
//        private readonly web_groupwareContext _context;

//        public T_INFOController(web_groupwareContext context)
//        {
//            _context = context;
//        }

//        // GET: T_INFO
//        public async Task<IActionResult> Index()
//        {
//              return _context.T_INFO != null ? 
//                          View(await _context.T_INFO.ToListAsync()) :
//                          Problem("Entity set 'web_groupwareContext.T_INFO'  is null.");
//        }

//        // GET: T_INFO/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null || _context.T_INFO == null)
//            {
//                return NotFound();
//            }

//            var t_INFO = await _context.T_INFO
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (t_INFO == null)
//            {
//                return NotFound();
//            }

//            return View(t_INFO);
//        }

//        // GET: T_INFO/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: T_INFO/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,info_cd,title,message,update_user,update_date")] T_INFO t_INFO)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(t_INFO);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(t_INFO);
//        }

//        // GET: T_INFO/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null || _context.T_INFO == null)
//            {
//                return NotFound();
//            }

//            var t_INFO = await _context.T_INFO.FindAsync(id);
//            if (t_INFO == null)
//            {
//                return NotFound();
//            }
//            return View(t_INFO);
//        }

//        // POST: T_INFO/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,info_cd,title,message,update_user,update_date")] T_INFO t_INFO)
//        {
//            if (id != t_INFO.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(t_INFO);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!T_INFOExists(t_INFO.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(t_INFO);
//        }

//        // GET: T_INFO/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null || _context.T_INFO == null)
//            {
//                return NotFound();
//            }

//            var t_INFO = await _context.T_INFO
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (t_INFO == null)
//            {
//                return NotFound();
//            }

//            return View(t_INFO);
//        }

//        // POST: T_INFO/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            if (_context.T_INFO == null)
//            {
//                return Problem("Entity set 'web_groupwareContext.T_INFO'  is null.");
//            }
//            var t_INFO = await _context.T_INFO.FindAsync(id);
//            if (t_INFO != null)
//            {
//                _context.T_INFO.Remove(t_INFO);
//            }
            
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool T_INFOExists(int id)
//        {
//          return (_context.T_INFO?.Any(e => e.Id == id)).GetValueOrDefault();
//        }
//    }
//}
