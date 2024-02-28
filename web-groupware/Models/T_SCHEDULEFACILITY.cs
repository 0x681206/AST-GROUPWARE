using Microsoft.EntityFrameworkCore;

namespace web_groupware.Models
{
    [PrimaryKey(nameof(schedule_no),nameof(place_cd), nameof(staf_cd))]
    public class T_SCHEDULEFACILITY
    {
        public int schedule_no { get; set; }

        public int place_cd { get; set; }

        public int staf_cd { get; set; }
        
    }

    public class FacilityDetailViewModel
    {
        public FacilityDetailViewModel(T_SCHEDULE Schedule, List<T_PLACEM> PlaceMs, List<T_STAFFM> StaffMs)
        {
            this.Schedule = Schedule;
            this.PlaceMs = PlaceMs;
            this.StaffMs = StaffMs;
        }

        public T_SCHEDULE Schedule;
        public List<T_PLACEM> PlaceMs;
        public List<T_STAFFM> StaffMs;
    }
}
