using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_groupware.Models
{
    public class T_AttendanceDate
    {
        [Key]
        public int id { get; set; }

        public int staf_cd { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string staf_name { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string state_num { get; set; }

        [DataType(DataType.Date)]
        public DateTime request_date { get; set; }

    }
}