using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace web_groupware.Models
{
    public class T_PLACEM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int place_cd { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string? place_name { get; set; }

        public int sort { get; set; }

        public T_PLACEM() { }
    }
}
