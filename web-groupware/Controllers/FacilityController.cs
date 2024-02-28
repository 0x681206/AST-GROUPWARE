using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Text;
using web_groupware.Data;
using web_groupware.Models;

#pragma warning disable CS8600, CS8601, CS8602, CS8604, CS8618

namespace web_groupware.Controllers
{
    //[Authorize]
    public class FacilityController : BaseController
    {
        public FacilityController(IConfiguration configuration, ILogger<BaseController> logger, web_groupwareContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
            : base(configuration, logger, context, hostingEnvironment, httpContextAccessor)
        {
        }
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var Schedule = new T_SCHEDULE();
            var PlaceMs = await _context.T_PLACEM.ToListAsync();
            var StaffMs = await _context.T_STAFFM.ToListAsync();

            return View(new FacilityDetailViewModel(Schedule, PlaceMs, StaffMs));
        }

        [HttpPost]
        public async Task<IActionResult> Create(string end_datetime, string memo, string place_cd, string schedule_type, string staf_cd, string start_datetime, string title)
        {
            T_SCHEDULE t_SCHEDULE = new T_SCHEDULE();
            var schedule_no = GetNextNo(Utilities.DataTypes.SCHEDULE_NO);
            t_SCHEDULE.schedule_no = schedule_no;
            t_SCHEDULE.schedule_type = int.Parse(schedule_type);
            t_SCHEDULE.start_datetime = DateTime.Parse(start_datetime);
            t_SCHEDULE.end_datetime = DateTime.Parse(end_datetime);
            t_SCHEDULE.title = title;
            t_SCHEDULE.memo = memo;
            _context.Add(t_SCHEDULE);
            await _context.SaveChangesAsync();
            try
            {
                T_SCHEDULEFACILITY t_SCHEDULEFACILITY = new T_SCHEDULEFACILITY();
                t_SCHEDULEFACILITY.schedule_no = schedule_no;
                t_SCHEDULEFACILITY.staf_cd = int.Parse(staf_cd);
                t_SCHEDULEFACILITY.place_cd = int.Parse(place_cd);
                _context.Add(t_SCHEDULEFACILITY);
                await _context.SaveChangesAsync();
                return Ok(schedule_no);
            }
            catch (Exception)
            {
                return StatusCode(500,  "An error occurred while creating the schedule.");
            }
        }

        public async Task<IActionResult> GetPersonal()
        {
            var facilityData = new List<object>();

            var facilityList = await _context.T_SCHEDULEFACILITY.ToListAsync();

            foreach (T_SCHEDULEFACILITY item in facilityList) {
                var schedule = await _context.T_SCHEDULE.FirstOrDefaultAsync(m => m.schedule_no == item.schedule_no);
                var place = await _context.T_PLACEM.FirstOrDefaultAsync(m => m.place_cd == item.place_cd);
                var user = await _context.T_STAFFM.FirstOrDefaultAsync(m => m.staf_cd == item.staf_cd);
                facilityData.Add(new { schedule, place, user });
            }
            
            return Ok(facilityData);

        }

        public async Task<IActionResult> Edit(string edit_schedule_no, string end_datetime, string memo, string place_cd, string schedule_type, string staf_cd, string start_datetime, string title)
        {
            try
            {
                var existingSchedule = await _context.T_SCHEDULE.FirstOrDefaultAsync(m => m.schedule_no == int.Parse(edit_schedule_no));
                    
                existingSchedule.schedule_type = int.Parse(schedule_type) > 0 ? int.Parse(schedule_type) : existingSchedule.schedule_type;
                
                existingSchedule.start_datetime = DateTime.Parse(start_datetime) != null ? DateTime.Parse(start_datetime) : existingSchedule.start_datetime;
                existingSchedule.end_datetime = DateTime.Parse(end_datetime) != null ? DateTime.Parse(end_datetime) : existingSchedule.end_datetime;
                existingSchedule.title = (title != null ? title : existingSchedule.title);
                existingSchedule.memo = (memo != null ? memo : existingSchedule.memo);
                _context.Update(existingSchedule);
                await _context.SaveChangesAsync();

                var existingFacilitySchedules = _context.T_SCHEDULEFACILITY
                    .Where(item => item.schedule_no == int.Parse(edit_schedule_no))
                    .ToList();

                _context.RemoveRange(existingFacilitySchedules);
                    
                T_SCHEDULEFACILITY t_SCHEDULEFACILITY = new T_SCHEDULEFACILITY();
                t_SCHEDULEFACILITY.schedule_no = int.Parse(edit_schedule_no);
                t_SCHEDULEFACILITY.staf_cd = int.Parse(staf_cd);
                t_SCHEDULEFACILITY.place_cd = int.Parse(place_cd);
                _context.Add(t_SCHEDULEFACILITY);
                await _context.SaveChangesAsync();

                return Ok(existingSchedule);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "An error occurred while creating the schedule.111");
            }
        }

        public async Task<IActionResult> Delete(string schedule_no) {
            var existingFacilitySchedules = _context.T_SCHEDULEFACILITY
                    .Where(item => item.schedule_no == int.Parse(schedule_no))
                    .ToList();

            _context.RemoveRange(existingFacilitySchedules);

            var existingSchedule = await _context.T_SCHEDULE.FirstOrDefaultAsync(m => m.schedule_no == int.Parse(schedule_no));
            _context.Remove(existingSchedule);

            await _context.SaveChangesAsync();
            return Ok("success");
        }




        [HttpPost]
        public async Task<IActionResult> GetScheduleInfo(string schedule_no)
        {
            try
            {
                var schedule = await _context.T_SCHEDULE.FirstOrDefaultAsync(m => m.schedule_no == int.Parse(schedule_no));
                var facility = await _context.T_SCHEDULEFACILITY.FirstOrDefaultAsync(m => m.schedule_no == int.Parse(schedule_no));

                return Ok(new { schedule, facility });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
