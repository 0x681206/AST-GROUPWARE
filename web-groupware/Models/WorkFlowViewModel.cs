using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_groupware.Models
{
    public class WorkFlowDetail
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? filename { get; set; }
        public string? icon { get; set; }
        public int size { get; set; }
        public int type { get; set; }

        public string? update_user { get; set; }
        public int manager_status { get; set; }
        public int approver_status { get; set; }
        public string? comment { get; set; }
        public DateTime update_date { get; set; }
    }
    public class WorkFlowViewModel
    {
        public List<WorkFlowDetail>? fileList = new List<WorkFlowDetail>();
    }
}