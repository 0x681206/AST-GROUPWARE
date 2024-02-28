
using Common.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8600,CS8602,CS8604,CS8618
namespace web_groupware.Models
{
    public class ReportDetailViewModel
    {
        public int? mode { get; set; }
        public DateTime? cond_date { get; set; }
        public int? report_no { get; set; }
        [DisplayName("日報年月日")]

        [Required(ErrorMessage = Messages.REQUIRED)]
        public DateTime report_date { get; set; }
        [DisplayName("内容")]
        [Required(ErrorMessage = Messages.REQUIRED)]
        [MaxLength(1000, ErrorMessage = Messages.MAXLENGTH)]
        public string message { get; set; }
        public string? update_user { get; set; }
    public List<ReportDetail>? list_report { get; set; }
        public ReportDetail? report { get; set; }
    }
    public class ReportDetail
    {
        public int? comment_no { get; set; }
        public string? update_user { get; set; }
        public string? update_date { get; set; }
        [DisplayName("コメント")]
        [Required(ErrorMessage = Messages.REQUIRED)]
        [MaxLength(1000, ErrorMessage = Messages.MAXLENGTH)]
        public string message { get; set; }

    }

}