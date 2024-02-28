using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using web_groupware.Models;
using web_groupware.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace web_groupware.Controllers
{
    public class TodoController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        private const int FILE_TYPE_FILE = 0;
        private const int FILE_TYPE_FOLDER = 1;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="httpContextAccessor"></param>
        public TodoController(IConfiguration configuration, ILogger<BaseController> logger, web_groupwareContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
            : base(configuration, logger, context, hostingEnvironment, httpContextAccessor)
        {
            _environment = hostingEnvironment;
        }

        [Authorize]
        public IActionResult Index()
        {
            TodoViewModel model = new TodoViewModel();

            var items = _context.T_TODO.ToList();
            foreach (var item in items)
            {
                model.fileList.Add(new TodoDetail
                {
                    id = item.id,
                    sendUrl = item.sendUrl,
                    title = item.title,
                    description = item.description == null ? "" : item.description,
                    staf_cd = item.staf_cd,
                    public_set = item.public_set,
                    group_set = item.group_set,
                    deadline_set = item.deadline_set,
                    response_status = item.response_status
                });
            }

            return View(model);

        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoDetail model)
        {
            if (ModelState.IsValid)
            {
                var todo = new T_TODO
                {
                    id = model.id,
                    title = model.title,
                    description = model.description,
                    sendUrl = model.sendUrl,
                    staf_cd = model.staf_cd,
                    public_set = model.public_set,
                    group_set = model.group_set,
                    deadline_set = model.deadline_set,
                    response_status = model.response_status,

                };

                _context.T_TODO.Add(todo);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

   
    [HttpPost("fileSave")]
    public async Task<IActionResult> fileSave([FromBody] IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var uploadsPath = Path.Combine(_environment.WebRootPath, "fileSave");
            var filePath = Path.Combine(uploadsPath, file.FileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Ok();
        }

        return BadRequest("No file uploaded.");
    }


        public async Task<T_TODO> itemGet(int id)
        {

            var item = await _context.T_TODO.FirstOrDefaultAsync(m => m.id == id);

            return item;
        }

        [HttpPost]
        public async Task<T_TODO> Update([FromBody] UpdateModel model)
        {
            var existingWorkflow = await _context.T_TODO.FirstOrDefaultAsync(m => m.id == model.id);
            if (existingWorkflow != null)
            {
                existingWorkflow.title = model.title;
                existingWorkflow.description = model.description;
                existingWorkflow.public_set = model.public_set;
                existingWorkflow.group_set = model.group_set;
                existingWorkflow.deadline_set = model.deadline_set;
                existingWorkflow.response_status = model.response_status;

                await _context.SaveChangesAsync();
                return existingWorkflow;
            }

            return new T_TODO(); // Return null or handle the case when the workflow doesn't exist
        }

        [HttpGet]
        public async Task<List<T_TODO>> FilterResponse(int value)
        {
            var items = await _context.T_TODO.Where(m => m.response_status == value).ToListAsync();
            return items;
        }

    }
}

