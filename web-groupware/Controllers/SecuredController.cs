using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics;
using web_groupware.Data;
using web_groupware.Models;

namespace web_groupware.Controllers
{
    [Authorize]
    public class SecuredController : BaseController
    {
        public SecuredController(IConfiguration configuration, ILogger<BaseController> logger, web_groupwareContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
            : base(configuration, logger, context, hostingEnvironment, httpContextAccessor)
        {
        }

        public async Task<IActionResult> Index()
        {
            return _context.T_SCHEDULE != null ?
                        View(await _context.T_SCHEDULE.ToListAsync()) :
                        Problem("Entity set 'web_groupwareContext.T_SCHEDULE'  is null.");
        }

        public async Task<IActionResult> Schedule(int schedule_no)
        {
            List<T_STAFFM> t_STAFFMs = await _context.T_STAFFM.ToListAsync();
            List<T_PLACEM> t_PLACEMs = await _context.T_PLACEM.ToListAsync();
            _logger.LogInformation(1, schedule_no + "schedule_no");
            ScheduleViewModel scheduleViewModel = new ScheduleViewModel();
            T_SCHEDULE? t_SCHEDULE = await _context.T_SCHEDULE.FirstOrDefaultAsync(m => m.schedule_no == schedule_no);
            List<T_SCHEDULEPEOPLE> t_SCHEDULEPEOPLEs = await _context.T_SCHEDULEPEOPLE.Where(m => m.schedule_no == schedule_no).ToListAsync();
            List<T_SCHEDULEPLACE>? t_SCHEDULEPLACEs = await _context.T_SCHEDULEPLACE.Where(m => m.schedule_no == schedule_no).ToListAsync();
            if (t_SCHEDULE != null)
            {
                scheduleViewModel.schedule_no = schedule_no;
                scheduleViewModel.schedule_type = t_SCHEDULE.schedule_type;
                scheduleViewModel.allday = t_SCHEDULE.allday;
                scheduleViewModel.start_datetime = t_SCHEDULE.start_datetime ?? DateTime.UtcNow;
                scheduleViewModel.end_datetime = t_SCHEDULE.end_datetime ?? DateTime.UtcNow;
                scheduleViewModel.title = t_SCHEDULE.title ?? "";
                scheduleViewModel.memo = t_SCHEDULE.memo ?? "";
                scheduleViewModel.People = t_SCHEDULEPEOPLEs;
                scheduleViewModel.Places = t_SCHEDULEPLACEs;
                scheduleViewModel.T_STAFFM = t_STAFFMs;
                scheduleViewModel.T_PLACEM = t_PLACEMs;
                return View(scheduleViewModel);
            }
            return View();
        }

        public async Task<IActionResult> Group_month()
        {
            GroupViewModel groupViewModel = new GroupViewModel();
            groupViewModel.T_GROUPM = await _context.T_GROUPM.ToListAsync();
            groupViewModel.T_PLACEM = await _context.T_PLACEM.ToListAsync();
            return View(groupViewModel);
        }

        public async Task<IActionResult> Group_week()
        {
            GroupViewModel groupViewModel = new GroupViewModel();
            groupViewModel.T_GROUPM = await _context.T_GROUPM.ToListAsync();
            groupViewModel.T_PLACEM = await _context.T_PLACEM.ToListAsync();
            return View(groupViewModel);
        }

        public IActionResult Personal_month()
        {
            return View();
        }

        public IActionResult Add_gevent()
        {
            return View();
        }
        public async Task<IActionResult> Personal_week()
        {
            var staf_cd = int.Parse(HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value);
            var schedule = await _context.T_SCHEDULE.FirstOrDefaultAsync();
            Debug.WriteLine(JsonConvert.SerializeObject(schedule));
            if (schedule != null)
            {
                return View(schedule);
            }
            else
            {
                return View();
            }
        }

        //==================================================================================
        [HttpPost]
        public async Task<IActionResult> Get_personal()
        {
            var staf_cd = int.Parse(HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value);
            int[] noList = _context.T_SCHEDULEPEOPLE.Where(m => m.staf_cd==staf_cd).Select(m => m.schedule_no).ToArray();
            var schedules = await _context.T_SCHEDULE.Where(e => noList.Contains(e.schedule_no)).ToListAsync();
            //var schedules = await _context.T_SCHEDULE.ToListAsync();

            return Ok(schedules);
        }

        [HttpPost]
        public async Task<IActionResult> getGroup(string cond)
        {
            if (cond==null || cond == "G-0" || cond == "P-0")
            {
                return Ok(await _context.T_SCHEDULE.ToListAsync());
            }else if (cond.Split("-")[0] == "G")
            {
                int groupId = int.Parse(cond.Split("-")[1]);
                int[] staf_cds = _context.T_GROUPSTAFF.Where(m => m.group_cd == groupId).Select(m => m.staf_cd).ToArray();
                int[] noList1 = _context.T_SCHEDULEPEOPLE.Where(m => staf_cds.Contains(m.staf_cd)).Select(m => m.schedule_no).Distinct().ToArray();
                var schedules1 = await _context.T_SCHEDULE.Where(e => noList1.Contains(e.schedule_no)).ToListAsync();
                return Ok(schedules1);
            }else
            {
                int placeId = int.Parse(cond.Split("-")[1]);
                //int[] place_cds = _context.T_GROUPSTAFF.Where(m => m.group_cd == placeId).Select(m => m.staf_cd).ToArray();
                int[] noList2 = _context.T_SCHEDULEPLACE.Where(m => m.place_cd==placeId).Select(m => m.schedule_no).Distinct().ToArray();
                List<T_SCHEDULE> schedules2 = await _context.T_SCHEDULE.Where(e => noList2.Contains(e.schedule_no)).ToListAsync();
                return Ok(schedules2);
            }
        }

        public async Task<IActionResult> Create([FromBody] SimpleScheduleModel simpleScheduleModel)
        {
            T_SCHEDULE t_SCHEDULE = new T_SCHEDULE();
            var schedule_no = GetNextNo(Utilities.DataTypes.SCHEDULE_NO);
            t_SCHEDULE.schedule_no = schedule_no;
            t_SCHEDULE.schedule_type = simpleScheduleModel.schedule_type;
            t_SCHEDULE.allday = simpleScheduleModel.allday;
            t_SCHEDULE.start_datetime = simpleScheduleModel.start_datetime;
            t_SCHEDULE.end_datetime = simpleScheduleModel.end_datetime;
            t_SCHEDULE.title = simpleScheduleModel.title;
            t_SCHEDULE.memo = simpleScheduleModel.memo;
            _context.Add(t_SCHEDULE);
            await _context.SaveChangesAsync();
            try
            {
                var staf_cd = int.Parse(HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value);
                _context.Add(new T_SCHEDULEPEOPLE(schedule_no: schedule_no, staf_cd: staf_cd));
                await _context.SaveChangesAsync();
                return Ok(t_SCHEDULE);
            }
            catch (Exception)
            {
                // handle exception
                return StatusCode(500, t_SCHEDULE.schedule_no.ToString() + "An error occurred while creating the schedule.");
            }
        }

        public async Task<IActionResult> Schedule_Create()
        {
            List<T_STAFFM> t_STAFFMs = await _context.T_STAFFM.ToListAsync();
            List<T_PLACEM> t_PLACEMs = await _context.T_PLACEM.ToListAsync();
            ScheduleViewModel scheduleViewModel = new ScheduleViewModel();
            scheduleViewModel.T_STAFFM = t_STAFFMs;
            scheduleViewModel.T_PLACEM = t_PLACEMs;
            return View(scheduleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Schedule_Create_Act([FromBody] ScheduleViewModel scheduleViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    T_SCHEDULE t_SCHEDULE = new T_SCHEDULE();
                    int schedule_no = GetNextNo(Utilities.DataTypes.SCHEDULE_NO);
                    t_SCHEDULE.schedule_no = schedule_no;
                    t_SCHEDULE.schedule_type = scheduleViewModel.schedule_type;
                    t_SCHEDULE.allday = scheduleViewModel.allday;
                    t_SCHEDULE.start_datetime = scheduleViewModel.start_datetime;
                    t_SCHEDULE.end_datetime = scheduleViewModel.end_datetime;
                    t_SCHEDULE.title = scheduleViewModel.title;
                    t_SCHEDULE.memo = scheduleViewModel.memo;
                    _context.Add(t_SCHEDULE);
                    await _context.SaveChangesAsync();

                    var staf_cd = int.Parse(HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value);
                    List<T_SCHEDULEPEOPLE> people = new List<T_SCHEDULEPEOPLE>();
                    foreach(T_SCHEDULEPEOPLE item in scheduleViewModel.People) {
                        people.Add(new T_SCHEDULEPEOPLE(schedule_no: schedule_no, staf_cd: item.staf_cd));
                    }
                    _context.T_SCHEDULEPEOPLE.AddRange(people);
                    await _context.SaveChangesAsync(true);

                    List<T_SCHEDULEPLACE>? places = new List<T_SCHEDULEPLACE>();

                    if (scheduleViewModel.Places != null)
                    {
                        foreach (T_SCHEDULEPLACE item in scheduleViewModel.Places)
                        {
                            places.Add(new T_SCHEDULEPLACE(schedule_no: schedule_no, place_cd: item.place_cd));
                        }
                        _context.T_SCHEDULEPLACE.AddRange(places);
                        await _context.SaveChangesAsync(true);
                    }
                    return RedirectToAction("Personal_week");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return StatusCode(500, ex);
                }
            }
            return StatusCode(500, "An error occurred while creating the schedule.");
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] T_SCHEDULE t_SCHEDULE)
        {
            if (t_SCHEDULE == null || t_SCHEDULE.schedule_no <= 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSchedule = await _context.T_SCHEDULE.FindAsync(t_SCHEDULE.schedule_no);

                    if (existingSchedule == null)
                    {
                        return NotFound();
                    }
                    existingSchedule.schedule_type = (t_SCHEDULE.schedule_type > 0 ? t_SCHEDULE.schedule_type : existingSchedule.schedule_type);
                    existingSchedule.allday = t_SCHEDULE.allday;
                    existingSchedule.start_datetime = (t_SCHEDULE.start_datetime != null ? t_SCHEDULE.start_datetime : existingSchedule.start_datetime);
                    existingSchedule.end_datetime = (t_SCHEDULE.end_datetime != null ? t_SCHEDULE.end_datetime : existingSchedule.end_datetime);
                    existingSchedule.title = (t_SCHEDULE.title != null ? t_SCHEDULE.title : existingSchedule.title);
                    existingSchedule.memo = (t_SCHEDULE.memo != null ? t_SCHEDULE.memo : existingSchedule.memo);
                    //existingSchedule.place_cd = (t_SCHEDULE.place_cd != null ? t_SCHEDULE.place_cd : existingSchedule.place_cd);
                    _context.Update(existingSchedule);
                    await _context.SaveChangesAsync();

                    var staf_cd = int.Parse(HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value);
                    return Ok(existingSchedule);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return StatusCode(500, "An error occurred while creating the schedule.111");
                }
            }
            return StatusCode(500, "An error occurred while creating the schedule.");
        }

        public async Task<IActionResult> Schedule_edit(int schedule_no)
        {
            List<T_STAFFM> t_STAFFMs = await _context.T_STAFFM.ToListAsync();
            List<T_PLACEM> t_PLACEMs = await _context.T_PLACEM.ToListAsync();
            _logger.LogInformation(1, schedule_no + "schedule_no");
            ScheduleViewModel scheduleViewModel = new ScheduleViewModel();
            T_SCHEDULE? t_SCHEDULE = await _context.T_SCHEDULE.FirstOrDefaultAsync(m => m.schedule_no == schedule_no);
            List<T_SCHEDULEPEOPLE> t_SCHEDULEPEOPLEs = await _context.T_SCHEDULEPEOPLE.Where(m => m.schedule_no == schedule_no).ToListAsync();
            List<T_SCHEDULEPLACE>? t_SCHEDULEPLACEs = await _context.T_SCHEDULEPLACE.Where(m => m.schedule_no == schedule_no).ToListAsync();
            if (t_SCHEDULE != null)
            {
                scheduleViewModel.schedule_no = schedule_no;
                scheduleViewModel.schedule_type = t_SCHEDULE.schedule_type;
                scheduleViewModel.allday = t_SCHEDULE.allday;
                scheduleViewModel.start_datetime = t_SCHEDULE.start_datetime ?? DateTime.UtcNow;
                scheduleViewModel.end_datetime = t_SCHEDULE.end_datetime ?? DateTime.UtcNow;
                scheduleViewModel.title = t_SCHEDULE.title??"";
                scheduleViewModel.memo = t_SCHEDULE.memo??"";
                scheduleViewModel.People = t_SCHEDULEPEOPLEs;
                scheduleViewModel.Places = t_SCHEDULEPLACEs;
                scheduleViewModel.T_STAFFM = t_STAFFMs;
                scheduleViewModel.T_PLACEM = t_PLACEMs;
                return View(scheduleViewModel);
            }
            return View(new ScheduleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> OneScheduleEdit([FromBody] ScheduleViewModel scheduleViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingSchedule = await _context.T_SCHEDULE.FindAsync(scheduleViewModel.schedule_no);
                    if (existingSchedule == null)
                    {
                        return NotFound();
                    }
                    existingSchedule.schedule_type = (scheduleViewModel.schedule_type > 0 ? scheduleViewModel.schedule_type : existingSchedule.schedule_type);
                    existingSchedule.allday = scheduleViewModel.allday;
                    existingSchedule.start_datetime = scheduleViewModel.start_datetime;
                    existingSchedule.end_datetime = scheduleViewModel.end_datetime;
                    existingSchedule.title = (scheduleViewModel.title != null ? scheduleViewModel.title : existingSchedule.title);
                    existingSchedule.memo = (scheduleViewModel.memo != null ? scheduleViewModel.memo : existingSchedule.memo);
                    _context.Update(existingSchedule);
                    await _context.SaveChangesAsync();

                    var staf_cd = int.Parse(HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value);
                    List<T_SCHEDULEPEOPLE> people = scheduleViewModel.People;
                    List<T_SCHEDULEPLACE>? places = scheduleViewModel.Places;

                    List<T_SCHEDULEPEOPLE> peopleToDelete = _context.T_SCHEDULEPEOPLE.Where(m => m.schedule_no == scheduleViewModel.schedule_no).ToList();
                    _context.T_SCHEDULEPEOPLE.RemoveRange(peopleToDelete);
                    await _context.SaveChangesAsync(true);
                    _context.T_SCHEDULEPEOPLE.AddRange(people);
                    await _context.SaveChangesAsync(true);

                    if (places != null)
                    {
                        List<T_SCHEDULEPLACE> placesToDelete = _context.T_SCHEDULEPLACE.Where(m => m.schedule_no == scheduleViewModel.schedule_no).ToList();
                        _context.T_SCHEDULEPLACE.RemoveRange(placesToDelete);
                        await _context.SaveChangesAsync(true);
                        _context.T_SCHEDULEPLACE.AddRange(places);
                        await _context.SaveChangesAsync(true);
                    }

                    return Ok(places);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return StatusCode(500, ex);
                }
            }
            return StatusCode(500, "An error occurred while creating the schedule.");
        }

        private bool T_SCHEDULEExists(int id)
        {
            return (_context.T_SCHEDULE?.Any(e => e.schedule_no == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var eventToDelete = await _context.T_SCHEDULE.FirstOrDefaultAsync(m => m.schedule_no == id);
            if (eventToDelete == null)
            {
                return StatusCode(500, id.ToString() + "-An error occurred while deleting the schedule.");
            }
            _context.T_SCHEDULE.Remove(eventToDelete);
            await _context.SaveChangesAsync();
            
            var schedulePeople = await _context.T_SCHEDULEPEOPLE.Where(m => m.schedule_no == id).ToListAsync();
            _context.T_SCHEDULEPEOPLE.RemoveRange(schedulePeople);
            await _context.SaveChangesAsync();
            
            var schedulePlace = await _context.T_SCHEDULEPLACE.Where(m => m.schedule_no == id).ToListAsync();
            _context.T_SCHEDULEPLACE.RemoveRange(schedulePlace);
            await _context.SaveChangesAsync();

            return RedirectToAction("Personal_week");  // Return a success response
        }
    }
}