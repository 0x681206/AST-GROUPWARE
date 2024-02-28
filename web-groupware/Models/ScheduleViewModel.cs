using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_groupware.Models
{
    public class ScheduleViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int schedule_no { get; set; }

        [DisplayName("スケジュールタイプ")]
 　　　　[Required(ErrorMessage = Common.Constants.Messages.REQUIRED)]
        public int schedule_type { get; set; }

        [DisplayName("終日")]
        public bool allday { get; set; }

        [DisplayName("開始時間")]
        public DateTime start_datetime { get; set; }

        [DisplayName("終了時間")]
        public DateTime end_datetime { get; set; }

        [DisplayName("タイトル")]
        [Required(ErrorMessage = Common.Constants.Messages.REQUIRED)]
        public string title { get; set; }

        [DisplayName("メモ")]
        [Required(ErrorMessage = Common.Constants.Messages.REQUIRED)]
        public string memo { get; set; }

        [DisplayName("参加者")]
        [NotMapped]
        public List<T_SCHEDULEPEOPLE> People { get; set; }

        [DisplayName("施設")]
        [NotMapped]
        public List<T_SCHEDULEPLACE>? Places { get; set; }

        [DisplayName("全スタッフ")]
        [NotMapped]
        public List<T_STAFFM>? T_STAFFM { get; set; }

        [DisplayName("全施設")]
        [NotMapped]
        public List<T_PLACEM>? T_PLACEM { get; set; }

        //public ScheduleViewModel()
        //{
        //    allday = false;
        //    start_datetime = DateTime.Now;
        //    end_datetime = DateTime.Now;
        //    title = string.Empty;
        //    memo = string.Empty;
        //    People = new List<T_SCHEDULEPEOPLE> { };
        //    Places = new List<T_SCHEDULEPLACE> { };
        //}
    }
}
