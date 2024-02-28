using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using web_groupware.Models;
using web_groupware.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8600, CS8601, CS8602, CS8604, CS8618, CS8629
namespace web_groupware.Controllers
{
    [Authorize]
    public class WorkFlowController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        private const int FILE_TYPE_FILE = 0;

        public WorkFlowController(IConfiguration configuration, ILogger<BaseController> logger, web_groupwareContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
            : base(configuration, logger, context, hostingEnvironment, httpContextAccessor)
        {
            _environment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            WorkFlowViewModel model = new WorkFlowViewModel();
            var userId = User.FindFirstValue(Utilities.ClaimTypes.STAF_CD);
            var user = _context.T_STAFFM.FirstOrDefaultAsync(m => m.staf_cd == int.Parse(userId));
            var workflow_auth = user.Result.workflow_auth;
            if (userId == null || _context.T_WORKFLOW == null)
            {
                return Problem(userId);
            }
            if (workflow_auth == 0)
            {
                var items = _context.T_WORKFLOW.Where(item => item.update_user == userId).ToList();
                foreach (var item in items)
                {
                    model.fileList.Add(new WorkFlowDetail
                    {
                        id = item.id,
                        filename = item.filename,
                        title = item.title,
                        description = item.description,
                        icon = item.icon,
                        size = item.size,
                        type = item.type,
                        update_date = item.update_date,
                        manager_status = item.manager_status,
                        approver_status = item.approver_status
                    });
                }
            }
            else if (workflow_auth == 1)
            {
                var items = _context.T_WORKFLOW.Where(item => item.manager_status != 0).ToList();
                foreach (var item in items)
                {
                    model.fileList.Add(new WorkFlowDetail
                    {
                        id = item.id,
                        filename = item.filename,
                        title = item.title,
                        description = item.description,
                        icon = item.icon,
                        size = item.size,
                        type = item.type,
                        update_date = item.update_date,
                        manager_status = item.manager_status,
                        approver_status = item.approver_status
                    });
                }
            } else
            {
                var items = _context.T_WORKFLOW.Where(item => item.manager_status == 2).ToList();
                foreach (var item in items)
                {
                    model.fileList.Add(new WorkFlowDetail
                    {
                        id = item.id,
                        filename = item.filename,
                        title = item.title,
                        description = item.description,
                        icon = item.icon,
                        size = item.size,
                        type = item.type,
                        update_date = item.update_date,
                        manager_status = item.manager_status,
                        approver_status = item.approver_status
                    });
                }
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, string title, string description)
        {
            if (file != null)
            {
                var uploadDir = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                var fileToUpload = Path.Combine(_environment.WebRootPath, "uploads", file.FileName);
                using (var fileStream = new FileStream(fileToUpload, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            using (IDbContextTransaction tran = _context.Database.BeginTransaction())
            {
                try
                {
                    int idx = file?.FileName.LastIndexOf('.') ?? -1;
                    var extension = "";
                    if (idx >= 0)
                        extension = file.FileName.Substring(idx + 1);

                    var record_new = new T_WORKFLOW
                    {
                        id = GetNextNo(Utilities.DataTypes.WORKFLOW_NO),
                        filename = file?.FileName,
                        title = title,
                        description = description,
                        icon = extension.IsNullOrEmpty() ? "" : extension + ".svg",
                        size = file != null ? (int)file.Length : 0,
                        type = FILE_TYPE_FILE,
                        manager_status = 0,
                        approver_status = 0,
                        comment = null,
                        update_user = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value,
                        manager = null,
                        approver = null,
                        update_date = DateTime.Now
                    };

                    _context.T_WORKFLOW.Add(record_new);
                    await _context.SaveChangesAsync();

                    tran.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                    tran.Dispose();
                    throw;
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> ApplyWorkFlow(int id, int manager_status, int approver_status, string comment)
        {
            if (id < 1 || _context.T_WORKFLOW == null)
            {
                return RedirectToAction("Index");
            }
            var workFlowDetail = await _context.T_WORKFLOW.FindAsync(id);
            if(workFlowDetail != null)
            {
                using (IDbContextTransaction tran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        workFlowDetail.manager_status = manager_status;
                        workFlowDetail.approver_status = approver_status;
                        workFlowDetail.comment = comment;
                        if(approver_status == 1) { 
                            workFlowDetail.manager = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                        } else if(approver_status > 1)
                        {
                            workFlowDetail.approver = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                        }

                        _context.T_WORKFLOW.Update(workFlowDetail);
                        await _context.SaveChangesAsync();

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);
                        tran.Dispose();
                        throw;
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string title, string description)
        {
            if (id < 1 || _context.T_WORKFLOW == null)
            {
                return RedirectToAction("Index");
            }

            var fileDetail = await _context.T_WORKFLOW.FindAsync(id);
            if (fileDetail != null)
            {
                using (IDbContextTransaction tran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        fileDetail.title = title;
                        fileDetail.description = description;

                        _context.T_WORKFLOW.Update(fileDetail);
                        await _context.SaveChangesAsync();

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);
                        tran.Dispose();
                        throw;
                    }
                }
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> GetSelectedData(int id)
        {
            var userId = User.FindFirstValue(Utilities.ClaimTypes.STAF_CD);
            var user = await _context.T_STAFFM.FirstOrDefaultAsync(m => m.staf_cd == int.Parse(userId));
            var workflow_auth = user.workflow_auth;
            var workFlowDetail = await _context.T_WORKFLOW.FindAsync(id);
            var result = new
            {
                workflow_auth,
                workFlowDetail
            };
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.T_WORKFLOW == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                var fileDetail = await _context.T_WORKFLOW.FindAsync(id);
                if (fileDetail != null)
                {
                    if (fileDetail.type == FILE_TYPE_FILE)
                    {
                        var path = Path.Combine(_environment.WebRootPath, "uploads", fileDetail.filename);
                        var fileDel = new FileInfo(path);
                        fileDel.Delete();
                    }
                    _context.T_WORKFLOW.Remove(fileDetail);
                    await _context.SaveChangesAsync();
                }
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
        public async Task<IActionResult> DownloadFile(int? id)
        {
            try
            {
                var fileDetail = await _context.T_WORKFLOW.FindAsync(id);
                if (fileDetail != null && fileDetail.type == FILE_TYPE_FILE)
                {
                    var path = Path.Combine(_environment.WebRootPath, "uploads", fileDetail.filename);
                    var content = await System.IO.File.ReadAllBytesAsync(path);
                    new FileExtensionContentTypeProvider()
                                    .TryGetContentType(fileDetail.filename, out string contentType);
                    if (contentType == null) contentType = System.Net.Mime.MediaTypeNames.Application.Octet;

                    return File(content, contentType, fileDetail.filename);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                throw;
            }
            return BadRequest();            
        }
    }
}