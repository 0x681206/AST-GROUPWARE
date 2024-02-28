using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_groupware.Data;
using web_groupware.Models;

#pragma warning disable CS8600, CS8601, CS8602, CS8604, CS8618, CS8629
namespace web_groupware.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
        public EmployeeController(IConfiguration configuration, ILogger<BaseController> logger, web_groupwareContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor) : base(configuration, logger, context, hostingEnvironment, httpContextAccessor) { }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await _context.T_STAFFM.ToListAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                var record = await _context.T_STAFFM.FirstOrDefaultAsync(m => m.staf_cd == id);
                if (record == null)
                {
                    return RedirectToAction("Index");
                }
                var model = new T_STAFFM();
                model.staf_cd = record.staf_cd;
                model.staf_name = record.staf_name;
                model.password = record.password;
                model.mail = record.mail;
                model.auth_admin = record.auth_admin;
                model.workflow_auth = record.workflow_auth;
                model.update_user = record.update_user;
                model.update_date = record.update_date;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(T_STAFFM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", Common.Constants.Message_change.FAILURE_001);
                    return View(model);
                }
                var record = await _context.T_STAFFM.FirstOrDefaultAsync(x => x.staf_cd == model.staf_cd);
                if (record == null)
                {
                    ModelState.AddModelError("", "存在しないグループコードです。");
                    return View(model);
                }
                record.staf_cd = model.staf_cd;
                record.staf_name = model.staf_name;
                record.password = model.password;
                record.auth_admin = model.auth_admin;
                record.workflow_auth = model.workflow_auth;
                record.update_user = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                record.update_date = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var record = await _context.T_STAFFM.FirstOrDefaultAsync(m => m.staf_cd == id);
                if (record == null)
                {
                    return RedirectToAction("Index");
                }
                var model = new T_STAFFM();
                model.staf_cd = record.staf_cd;
                model.staf_name = record.staf_name;
                model.password = record.password;
                model.mail = record.mail;
                model.auth_admin = record.auth_admin;
                model.workflow_auth = record.workflow_auth;
                model.update_user = record.update_user;
                model.update_date = record.update_date;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(T_STAFFM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", Common.Constants.Message_delete.FAILURE_001);
                    return View(model);
                }
                var record = await _context.T_STAFFM.FirstOrDefaultAsync(x => x.staf_cd == model.staf_cd);
                if (record == null)
                {
                    ModelState.AddModelError("", "存在しないグループコードです。");
                    return View(model);
                }
                _context.T_STAFFM.Remove(record);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
    }
}
