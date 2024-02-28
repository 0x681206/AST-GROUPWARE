#pragma warning disable CS8600,CS8602,CS8604,CS8618
namespace web_groupware.Models
{
    public class ReportViewModel
    {
        public DateTime? cond_date { get; set; }

        public List<Report> list_report = new List<Report>();
    }
    public class Report
    {
        public int report_no { get; set; }
        public string name { get; set; }
        public string report_date { get; set; }
        public string update_date { get; set; }
        public string message { get; set; }
        public string count { get; set; }
        public bool isMe { get; set; }

    }

}