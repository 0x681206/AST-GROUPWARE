using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Constants;
using Newtonsoft.Json;
#pragma warning disable CS8600,CS8602,CS8604,CS8618
namespace web_groupware.Models
{
    public class RestrationReportViewModel
    {
        public DateTime? cond_leaving_date_from { get; set; }
        public DateTime? cond_leaving_date_to { get; set; }
        [MaxLength(10, ErrorMessage = Messages.MAXLENGTH)]
        public string? cond_staf_cd { get; set; }


        public List<RestrationReport> list_report = new List<RestrationReport>();
    }
    public class RestrationReport
    {
        public int hachu_no { get; set; }
        public string bukken_name { get; set; }
        public int room_no { get; set; }
        public string leaving_date { get; set; }
        public int leaving_staffcd { get; set; }
        public string restoration_date { get; set; }
        public string staf_name { get; set; }

    }
}