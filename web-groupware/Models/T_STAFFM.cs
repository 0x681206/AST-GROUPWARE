using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#pragma warning disable CS8600,CS8601,CS8602,CS8604,CS8618,CS8629

namespace web_groupware.Models
{
    public class T_STAFFM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("社員番号")]
        public int staf_cd { get; set; }

        [Column(TypeName = "varchar(20)")]
        [DisplayName("パスワード")]
        public string? password { get; set; }

        [DisplayName("社員名名")]
        [Column(TypeName = "nvarchar(10)")]
        public string? staf_name { get; set; }

        [Range(0, 1)]
        [DisplayName("管理者権限")]
        public int auth_admin { get; set; } = 0;
        [DisplayName("承認者権限")]
        public int workflow_auth { get; set; } = 0;

        [Column(TypeName = "varchar(50)")]
        [DisplayName("メールアドレス")]
        [Required(ErrorMessage = Common.Constants.Messages.REQUIRED)]
        [EmailAddress(ErrorMessage = Common.Constants.Messages.EMAILADDRESS)]
        public string mail {  get; set; }

        [DisplayName("更新者")]
        [Column(TypeName = "varchar(10)")]
        public string update_user { get; set; }

        [DisplayName("更新日時")]
        [Column(TypeName = "datetime2(7)")]
        public DateTime update_date { get; set; }
        public T_STAFFM() {
            auth_admin = 0;
            mail = string.Empty;
            update_user = string.Empty;
            update_date = DateTime.Now;
        }
    }
}
