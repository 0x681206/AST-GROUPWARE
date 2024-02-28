using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace web_groupware.Models
{
    public class T_SCHEDULE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int schedule_no { get; set; }
        [Required(ErrorMessage = "必須項目です。")]
        public int schedule_type { get; set; }

        public bool allday { get; set; } = false;

        [Required(ErrorMessage = "必須項目です。")]
        [Column(TypeName = "datetime2(7)")]
        public DateTime? start_datetime { get; set; }
    
        [Required(ErrorMessage = "必須項目です。")]
        [Column(TypeName = "datetime2(7)")]
        public DateTime? end_datetime { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? title { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? memo { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string update_user { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime update_date { get; set; }

        public T_SCHEDULE()
        {
            update_user = string.Empty;
        }
    }
}
