using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#pragma warning disable CS8600,CS8602,CS8604,CS8618
namespace web_groupware.Models
{
    public class T_INFO
    {
        [Key]
        public int info_cd { get; set; }

        [Column(TypeName = "nvarchar(40)")]
        public string? title { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? message { get; set; } = "";

        [Column(TypeName = "varchar(10)")]
        public string update_user { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime update_date { get; set; }

    }
}