using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_groupware.Models
{
    public class T_NO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int data_type { get; set; }
        public int no { get; set; }

    }
}