using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_groupware.Models
{
    public class TodoDetail
    {
        public int id { get; set; }
        [Column(TypeName = "nvarchar(64)")]
        public string? title { get; set; }
        [Column(TypeName = "nvarchar(64)")]
        public string? description { get; set; }
        [Column(TypeName = "nvarchar(64)")]
        public string? sendUrl { get; set; }
        
        public int staf_cd { get; set; }
        public int public_set { get; set; }
        public int group_set { get; set; }
        public int deadline_set { get; set; }
        public int response_status { get; set; }
    }
    public class TodoViewModel
    {
        public List<TodoDetail>? fileList = new List<TodoDetail>();
        public int id { get; set; }
        [Column(TypeName = "nvarchar(64)")]
        public string? title { get; set; }
        [Column(TypeName = "nvarchar(64)")]
        public string? description { get; set; }
        [Column(TypeName = "nvarchar(64)")]
        public string? sendUrl { get; set; }
        public int staf_cd { get; set; }
        public int public_set { get; set; }
        public int group_set { get; set; }
        public int deadline_set { get; set; }
        public int response_status { get; set; }
    }

    public class UpdateModel
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public int public_set { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public int group_set { get; set; }
        public int deadline_set { get; set; }
        public int response_status { get; set; }
    }

    public class filterResponseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int response_status {get; set;}
    }
}